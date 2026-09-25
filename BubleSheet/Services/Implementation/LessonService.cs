using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using MiniShop.Application.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class LessonService(IStudentLesson studentLesson,IQuestionBankService questionBankService,IExamService examService,IpdfService pdfService,IExam exam,IQuestionBank questionBank,IPdf pdf,IRandomExamTemplate randomExamTemplate, IAccount account,IAccountService accountService,IUnitOfWork unitOfWork,ILesson lesson,IJwtService jwtService,IstudentSubject studentsubject,IStudentAttempts studentAttempts) : ILessonService
    {
        private readonly ILesson _lesson = lesson;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IstudentSubject _studentsubject = studentsubject;
        private readonly IStudentAttempts _studentAttempts = studentAttempts;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAccountService _accountService = accountService;
        private readonly IAccount _account = account;
        private readonly IPdf _pdf = pdf;
        private readonly IRandomExamTemplate _randomExamTemplate = randomExamTemplate;
        private readonly IQuestionBank _QuestionBank= questionBank;
        private readonly IExam _exam = exam;
        private readonly IQuestionBankService _QuestionBankService = questionBankService;
        private readonly IExamService _examService = examService;
        private readonly IpdfService _pdfservice = pdfService;
        private readonly IStudentLesson _studentLesson = studentLesson;
        public async Task<List<LessonDto>> GetLessonBySubjectIdAsync(int SubjectId)
        {
            var lessons = await _lesson.GetLessonsBySubjectId(SubjectId);

            return lessons.Select(l => new LessonDto
            {
                Description = l.Description,
                LessonName = l.LessonName,
                LessonID = l.LessonID,
                Index = l.Index
            }).ToList();
        }
        public async Task<LessonDetailsDto> GetLessonDetails(int lessonId)
        {
            int studentId = _jwtService.GetCurrentStudentId();

            var lesson = await _lesson.GetLessonDetailsById(lessonId);

            if (lesson == null)
                throw new Exception("Lesson not found.");

            bool paid = await _studentsubject
                .IsStudentSubscribedToSubject(studentId, lesson.SubjectId);

            var student = await _account.GetStudentByIDAsync(studentId);
            bool isAdmin = _accountService.checkIfHeIsAdmin(student.Email);

            var studentLesson = lesson.StudentLessons
                .FirstOrDefault(sl => sl.StudentId == studentId);

            var StudentAttemptsExams = await _studentAttempts.GetAllStudentAttemptExamIds(studentId);
            var StudentAttemptsQuestionBank = await _studentAttempts.GetAllStudentAttemptQuestionBankIds(studentId);

            return new LessonDetailsDto
            {
                LessonID = lesson.LessonID,
                LessonName = lesson.LessonName,
                Description = lesson.Description,
                Index = lesson.Index,
                LessonStatue = studentLesson?.LessonStatue ?? LessonStatue.NotStart,
                subjectName = lesson.Subject.Name,
                Price = lesson.Subject.Price,
                Paid = paid,
                Exams = lesson.Exams.Where(e => e.StudentId == studentId || e.StudentId == null)
                .OrderByDescending(e=>e.CreatedAt)
                .Select(e => new ExamDto
                {
                    ExamId = e.ExamID,
                    Description = e.Description,
                    ExamName = e.ExamName,
                    HasOneFree = !StudentAttemptsExams.Contains(e.ExamID),
                    Duration = e.Duration,
                    QCount = e.ExamQuestions.Count
                }).ToList(),
                PDFs = lesson.pdfFiles.Select(p => new PDFDTO
                {
                    PdfId = p.PdfId,
                    Description = p.Description,
                    Space = p.Space,
                    PdfName = p.PdfName,
                }).ToList(),
                questionBanks = lesson.QuestionBanks.Where(q => isAdmin || q.IsActive).Select(q => new QuestionBankDTO
                {
                    BankId = q.BankId,
                    BankName = q.BankName,
                    IsFree = q.IsFree,
                    QCount = q.QCount,
                    Description = q.Description,
                    HasSolved = StudentAttemptsQuestionBank.Contains(q.BankId),
                    IsActive = q.IsActive,
                }).ToList()
            };
        }
        public async Task<LessonDto> AddLessonAsync(AddLessonDTO DTO)
        {
            var LastIndex = await _lesson.LastIndex(DTO.SubjectId);
            Lesson newLesson = new Lesson(DTO.Name,DTO.Description,LastIndex+1,DTO.SubjectId);
            var Lesson = await _lesson.AddAsync(newLesson);
            RandomExamTemplate examTemplate = new RandomExamTemplate(Lesson.LessonID, 10, TimeSpan.FromMinutes(10)); //Defult
            await _randomExamTemplate.AddAsync(examTemplate);
            return new LessonDto
            {
                Description = DTO.Description,
                LessonID = Lesson.LessonID,
                Index = LastIndex,
                LessonStatue = LessonStatue.NotStart,
                LessonName = DTO.Name
            };
        }
        public async Task<LessonDto> EditLesson(EditLessonDto Dto)
        {
            var Lesson = await _lesson.GetLessonDetailsById(Dto.LessonId);
            Lesson.Update(Dto.Name,Dto.Description);
            await _unitOfWork.SaveChangesAsync();
            return new LessonDto
            {
                Description = Dto.Description,
                LessonID = Lesson.LessonID,
                Index = Lesson.Index,
                LessonStatue = LessonStatue.NotStart,
                LessonName = Dto.Name
            };
        }
        public async Task DeleteLesson(int LessonId)
        {
            var Questionbanks = await _QuestionBank.GetAllQuestionBankIdsByLessonId(LessonId);
            var Exams = await _exam.GetAllExamIdsByLessonId(LessonId);

            await _pdfservice.DeletePdfsinLesson(LessonId);
            await _examService.DeleteExams(Exams);
            await _QuestionBankService.DeleteQuestionBanks(Questionbanks);
            await _randomExamTemplate.DeleteRandExam(LessonId);
            await _studentLesson.DeleteAllStudentLessonsByessonId(LessonId);
            await _lesson.DeleteLesson(LessonId);
        }
        public async Task DeleteLessons(List<int> LessonIDs)
        {
            foreach(int LessonID in LessonIDs)
            {
                await DeleteLesson(LessonID);
            }
            await _unitOfWork.SaveChangesAsync();
        }


    }
}
