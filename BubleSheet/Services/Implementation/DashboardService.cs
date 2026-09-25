using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using MiniShop.Application.Interfaces;
using System.Runtime.InteropServices.JavaScript;

namespace BubleSheet.Services.Implementation
{
    public class DashboardService(IQuestion question,IQuestionExam questionExam,IJwtService jwtService,IStudentAttempts studentAttempts,IStudentScore studentScore,IBunnyStorageService bunnyStorageService,IAccount account , IYear year , ISubject subject, IImgadService imgAdService) : IDashboardService
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly IStudentAttempts _studentAttempts = studentAttempts;
        private readonly IAccount _account = account;
        private readonly IYear _year = year;
        private readonly ISubject _sebject = subject;
        private readonly IImgadService _imgAdService = imgAdService;
        private readonly IBunnyStorageService _bunnyStorageService = bunnyStorageService;
        private readonly IStudentScore _studentScore = studentScore;
        private readonly IQuestionExam _questionExam = questionExam;
        private readonly IQuestion _question = question;
        public async Task<DataResponseDto> GetMainDataforStudent(int stuId)
        {
            var student = await _account.GetStudentByIDAsync(stuId);
            DataResponseDto dataResponse = new DataResponseDto
            {
                Name = student.StudentName,
                Email = student.Email,
                Gender = student.Gender ? "ذكر" : "انثى",
                phoneNumber = student.PhoneNumber,
                ParentphoneNumber = student.ParentPhoneNumber,
                School = student.School,
                Balance = student.Balance,
                LastLogin = student.LastLogin
            };
            return dataResponse;
        }
        public async Task<List<StudentTableDto>> GetTableStudent(
      int AcYearId,
      bool IsStudent)
        {
            var getStudentScore =
                await _studentScore.GetByAcYearIdAsync(AcYearId);

            var scoreByStudentId = getStudentScore
                .ToDictionary(x => x.StudentId, x => x.Score);

            var students = await _account.GetAllStudentsById(
                scoreByStudentId.Keys.ToList()
            );

            var result = students
                .Select(student => new StudentTableDto
                {
                    Name = student.StudentName,
                    Score = scoreByStudentId[student.StudentId],
                    SchoolName = student.School
                })
                .OrderByDescending(x => x.Score)
                .ToList();

            return result;
        }
        public async Task<(int, int, double)> GetStudentSolvedData()
        {
            var studentId = _jwtService.GetCurrentStudentId();

            var studentAttempts =
                await _studentAttempts.GetStudentAttempetsByStudentId(studentId);

            var examAttempts = studentAttempts
                .Where(x => x.IsSubmitted && x.SubmitType == submitType.Exam)
                .ToList();

            var QuestionBank = studentAttempts
                .Where(x => x.IsSubmitted && x.SubmitType == submitType.QuestionBank)
                .ToList();

            var numberOfExams = examAttempts.Count + QuestionBank.Count;

            var idsOfExams = examAttempts
     .Where(x => x.ExamId.HasValue)
     .Select(x => x.ExamId.Value)
     .Distinct()
     .ToList();

            var idsOfQBank = QuestionBank
                .Where(x => x.QuestionBankId.HasValue)
                .Select(x => x.QuestionBankId.Value)
                .Distinct()
                .ToList();

            var numberOfQuestions = 0;

            var Questions = await _question.GetQuestioIdsByBankId(idsOfQBank);

            numberOfQuestions = numberOfQuestions + Questions.Count; 

            if (idsOfExams.Count > 0)
            {
                numberOfQuestions =
                    await _questionExam.GetNumberOfQuestionsInExams(idsOfExams);
            }

            var totalScore = studentAttempts.Sum(s => s.Score);

            var percentage = numberOfQuestions > 0
                ? (totalScore * 100) / numberOfQuestions
                : 0;

            return (numberOfExams, numberOfQuestions, percentage);
        }
        public async Task<List<YearsResponseDto>> GetAllYearsByLevel(Levels level)
        {
            var years = await _year.GetYearsByLevel(level);

            return years.Select(y => new YearsResponseDto
            {
                yearId = y.AcademicYearId,
                YearName = y.Name,
                ImgLink =  y.imgLink == null ? "" : _bunnyStorageService.GenerateSecureUrl(y.imgLink)
            }).ToList();
        }
        public async Task<List<SubjectsDto>> GetAllSubjects(int id)
        {
            var subjects = await _sebject.GetAllSubjectsByYearId(id);

            return subjects.Select(s => new SubjectsDto
            {
                SubjectId = s.SubjectId,
                Name = s.Name,
                Description = s.Description,
                LessonCount = s.Lessons.Count,
                Level = s.academicYear.ACLevel,
                Price = s.Price
            }).ToList();
        }
        public async Task<AdminDataResponseDto> GetMainDataforAdmin(Levels level = Levels.level2)
        {
            return new AdminDataResponseDto
            {
                CountOfStudent = await _account.CountOfStudent(),
                Years = await GetAllYearsByLevel(level),
                imgAds = await _imgAdService.GetAllImgAds()
            };
        }
    }
}
