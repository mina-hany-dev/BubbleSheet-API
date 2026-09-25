using bubblesheet.Infrastracture.Dtos;
using bubblesheet.Infrastracture.Repos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using MiniShop.Application.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class StudentAttemptService(IExam exam,IQuestionBank questionBank,IStudentAttempts studentAttempt,IJwtService jwtService) : IStudentAttemptService 
    {
        private readonly IStudentAttempts _studentAttempt = studentAttempt;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IExam _exam = exam;
        private readonly IQuestionBank _questionBank = questionBank;
        
        public async Task<int> AddStudentAttemptAsync(StudentAttemptDTO attemptDTO)
        {
            int studentId = _jwtService.GetCurrentStudentId();

            StudentAttempt attempt;

            if (attemptDTO.submitType == submitType.Exam)
            {
                attempt = StudentAttempt.CreateExamAttempt(
                    studentId,
                    attemptDTO.Id);
            }
            else
            {
                attempt = StudentAttempt.CreateQuestionBankAttempt(
                    studentId,
                    attemptDTO.Id);
            }

            await _studentAttempt.AddAsync(attempt);
            return attempt.StudentAttemptId;
        }
        public async Task<bool> IsStudentCompeleteAllLesson(
     int studentId,
     int lessonId)
        {
            var allSubmittedAttempts =
                await _studentAttempt.GetStudentAttempetsByStudentId(studentId);

            var examIds =
                await _exam.GetAllExamIdsByLessonId(lessonId);

            var questionBankIds =
                await _questionBank.GetAllQuestionBankIdsByLessonId(lessonId);

            var submittedExamIds = allSubmittedAttempts
                .Where(x =>
                    x.IsSubmitted &&
                    x.SubmitType == submitType.Exam &&
                    x.ExamId.HasValue)
                .Select(x => x.ExamId!.Value)
                .ToHashSet();

            var submittedQuestionBankIds = allSubmittedAttempts
                .Where(x =>
                    x.IsSubmitted &&
                    x.SubmitType == submitType.QuestionBank &&
                    x.QuestionBankId.HasValue)
                .Select(x => x.QuestionBankId!.Value)
                .ToHashSet();

            var allExamsCompleted = examIds.All(id =>
                submittedExamIds.Contains(id));

            var allQuestionBanksCompleted = questionBankIds.All(id =>
                submittedQuestionBankIds.Contains(id));

            return allExamsCompleted && allQuestionBanksCompleted;
        }
    }
}
