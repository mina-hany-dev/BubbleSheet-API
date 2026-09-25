using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using MiniShop.Application.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class ExamService (ISubject subject,ILesson lesson,IStudentSubjectService studentSubjectService,IYear year,IStudentScore studentScore,IAccount account1,IStudentLessonService studentLessonService,IExam exam ,IStudentAnswer studentAnswer, IRandomExamTemplate randomExamTemplate , IUnitOfWork unitOfWork, IAccount account,IStudentAttempts attempts, IStudentAttempts studentAttempts , IJwtService jwtService, IStudentAttemptService studentAttemptService , IQuestionExam questionExam , IQuestionService questionService) : IExamService
    {
        private readonly IExam _exam = exam;
        private readonly ISubject _subject = subject;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IQuestionExam _questionExam = questionExam;
        private readonly IQuestionService _questionService = questionService;
        private readonly IStudentAttemptService _studentAttemptService = studentAttemptService;
        private readonly IStudentAttempts _studentAttempts = studentAttempts;
        private readonly IStudentAnswer _studentAnswer = studentAnswer;
        private readonly IRandomExamTemplate _randomExamTemplate = randomExamTemplate;
        private readonly IStudentLessonService _studentLessonService = studentLessonService;
        private readonly IStudentScore _studentScore = studentScore;
        private readonly IAccount _account = account1;
        private readonly ILesson _lesson = lesson;
        private readonly IStudentSubjectService _StudentSubjectService = studentSubjectService;
        private readonly IYear _year = year;
        public async Task<int> AddExamAsync(
    AddExamDto addExamDto,
    bool saveChanges = true)
        {
            if (addExamDto == null)
                throw new ArgumentNullException(nameof(addExamDto));

            var exam = new Exam(
                addExamDto.ExamName,
                addExamDto.Duration,
                addExamDto.Description,
                addExamDto.LessonId,
                addExamDto.StudentId
            );

            await _exam.AddAsyncExam(exam);

            if (saveChanges)
            {
                await _unitOfWork.SaveChangesAsync();
            }

            return exam.ExamID;
        }
        public async Task<ExamDto> EditExam(EditExamDto Dto)
        {
            var exam = await _exam.GetExamById(Dto.ExamId);
            exam.Update(Dto.ExamName, Dto.Duration, Dto.Description);
            await _unitOfWork.SaveChangesAsync();
            return new ExamDto
            {
                Description = Dto.Description,
                Duration = Dto.Duration,
                ExamId = Dto.ExamId,
                ExamName = Dto.ExamName,
            };
        }
        public async Task<int> CreateAnRandomExam(int LessonId)
        {
            int StudentId = _jwtService.GetCurrentStudentId();
            var Lesson = await _lesson.GetLessonDetailsById(LessonId);
            
            bool StudentIsPaid = await _StudentSubjectService.IsStudentSubscribedToSubject(StudentId, Lesson.SubjectId);
            if (!StudentIsPaid)
                throw new Exception("Not Access.");


            var RandomExamTemplate = await _randomExamTemplate.GetByLessonIdAsync(LessonId);
            int CountOfExam = await _exam.GetCountOfExamsForStudent(StudentId);
            AddExamDto addExamDto = new AddExamDto
            {
                LessonId = LessonId,
                ExamName = $"{CountOfExam+1} امتحان عشوائى مدفوع",
                Description = $"{CountOfExam + 1} امتحان عشوائى مدفوع",
                Duration = RandomExamTemplate.Duration,
                StudentId = StudentId,
            };
            int Examid = await AddExamAsync(addExamDto,true);
            await _questionService.GenerateQuestionsAsync(LessonId, RandomExamTemplate.QuestionCount,Examid);
            await _unitOfWork.SaveChangesAsync();
            return Examid;
        }
        public async Task<ExamQuestionWithAttemptID> GetExamWithQuestions(int examId)
        {
            int studentId = _jwtService.GetCurrentStudentId();

            var exam = await _exam.GetExamById(examId);

            if (exam == null)
                throw new Exception("Exam not found.");

            bool isTaken = await _studentAttempts
                .HasStudentTakenFreeAttemptExam(studentId, examId);

            if (isTaken)
                throw new Exception("This exam has already been taken.");

            var existAttempt = await _studentAttempts
                .GetAttemptHasNotSumbittedByIdForExam(studentId, examId);

            int attemptId;

            if (existAttempt != null)
            {
                attemptId = existAttempt.StudentAttemptId;
            }
            else
            {
                attemptId = await _studentAttemptService.AddStudentAttemptAsync(
                    new StudentAttemptDTO
                    {
                        Id = examId,
                        submitType = submitType.Exam
                    });
            }

            var questionIds = await _questionExam.GetIDQuestionsByExamId(examId);

            var questions = await _questionService.GetQuestionsByIds(questionIds);

            return new ExamQuestionWithAttemptID
            {
                ExamId = exam.ExamID,
                ExamName = exam.ExamName,
                Description = exam.Description,
                Duration = exam.Duration,
                QCount = exam.ExamQuestions.Count,
                Questions = questions,
                AttemptId = attemptId
            };
        }
        public async Task<ExamDto> GetExam(int ExamId)
        {
            var exam = await _exam.GetExamById(ExamId);
            if (exam == null)
                throw new Exception("Exam not found.");

            return new ExamDto
            {
                ExamId = exam.ExamID,
                ExamName = exam.ExamName,
                Description = exam.Description,
                Duration = exam.Duration,
                QCount = exam.ExamQuestions.Count
            };
        }

        public async Task<SumbitResponseDto> SubmitExam(SubmitDto submitDto)
        {
            int studentId = _jwtService.GetCurrentStudentId();

            var student = await _account.GetStudentByIDAsync(studentId);

            var attempt = await _studentAttempts.GetById(submitDto.StudentAttemptId);

            if (attempt == null)
                throw new Exception("Attempt not found.");

            if (attempt.StudentId != studentId)
                throw new Exception("Unauthorized.");

            if (attempt.IsSubmitted)
                throw new Exception("Exam has already been submitted.");

            var questionIds = submitDto.Answers
                .Select(x => x.QuestionId)
                .ToList();

            var questions = await _questionService.GetQuestionsByIds(questionIds);

            var studentAnswers = new List<StudentAnswer>();

            int correctAnswers = 0;

            foreach (var answer in submitDto.Answers)
            {
                var question = questions.FirstOrDefault(q => q.QuestionId == answer.QuestionId);

                if (question == null)
                    throw new Exception($"Question {answer.QuestionId} not found.");

                bool isCorrect = question.Choices
                    .Any(c => c.ChoiseId == answer.ChoiceId && c.IsCorrect);

                if (isCorrect)
                    correctAnswers++;

                studentAnswers.Add(new StudentAnswer(
                    submitDto.StudentAttemptId,
                    answer.QuestionId,
                    answer.ChoiceId,
                    isCorrect));
            }

            await _studentAnswer.AddRangeAsync(studentAnswers);

            attempt.Submit(correctAnswers);

            await _unitOfWork.SaveChangesAsync();


            if (!attempt.ExamId.HasValue)
                throw new Exception("Exam not found for this attempt.");

            var lessonId = await _exam.GetLessonIdByExamId(attempt.ExamId.Value);

            if (!lessonId.HasValue)
                throw new Exception("Lesson not found for this exam.");
            await _studentLessonService.DoUpdateLessonStatus(lessonId.Value);

            var AcYearId = await _year.GetAcademicYearIdByExamId(attempt.ExamId.Value);
            var studentScore = await _studentScore.GetByStudentAndYearAsync(studentId,AcYearId.Value);
            if(studentScore == null)
            {
                StudentScore newStudentScore = new StudentScore(AcYearId.Value,studentId);
                await _studentScore.AddAsync(newStudentScore);
                await _studentScore.SaveChangesAsync();
                newStudentScore.AddScore(correctAnswers);
            }
            else
            {
                studentScore.AddScore(correctAnswers);
            }
            await _studentScore.SaveChangesAsync();

            return new SumbitResponseDto
            {
                Score = correctAnswers,
                precentage = (int)correctAnswers * 100 / questions.Count
            };
        }


        public async Task DeleteExam(int id)
        {
            await _questionExam.DeleteAllQuestionFromExam(id);
            var AttemptsIds = await _studentAttempts.GetAllAttemptIDs(id, submitType.Exam,true);
            await _studentAttempts.RemoveRangeStudentAttempt(AttemptsIds);
            await _exam.DeleteAsyncExam(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteExams(List<int> IDs)
        {
            foreach (var id in IDs)
            {
                await DeleteExam(id);
            }
        }
        public async Task UpdateExamQuestions(UpdateExamQuestionsDto dto)
        {
            var exam = await _exam.GetExamById(dto.ExamId);

            if (exam == null)
                throw new Exception("Exam not found");

            var questionIds = dto.QuestionIds
                .Distinct()
                .ToList();

            await _questionExam.DeleteAllQuestionFromExam(dto.ExamId);

            await _questionExam.AddRangeAsync(
                questionIds,
                dto.ExamId
            );
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
