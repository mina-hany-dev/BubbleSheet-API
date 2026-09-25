using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using MiniShop.Application.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class ReviewService(IBunnyStorageService bunnyStorageService,IJwtService jwtService,IStudentAttempts studentAttempt,IStudentAnswer studentAnswer, IStudentAttemptService studentAttemptService , IQuestion question , IQuestionExam questionExam) : IReviewService
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly IStudentAttempts _studentAttempt = studentAttempt;
        private readonly IQuestion _question = question;
        private readonly IStudentAnswer _studentAnswer = studentAnswer;
        private readonly IBunnyStorageService _buunyStorageService = bunnyStorageService;
        public async Task<ReviewDTO?> ReviewQuestions(int Id, submitType Type)
        {
            int studentId = _jwtService.GetCurrentStudentId();

            int? attemptId =
                await _studentAttempt.GetAttemptIdByIdAndType(studentId, Id, Type);

            if (attemptId == null)
                return null;

            var Attempt = await _studentAttempt.GetById(attemptId.Value);

            var answers =
                await _studentAnswer.GetAllAnswersByAttemptId(attemptId.Value);

            var questionIds =
                answers.Select(x => x.QuestionId)
                       .Distinct()
                       .ToList();

            var questions =
                await _question.GetQuestionsByIds(questionIds);

            var responseQuestions = new List<ResponseQuestionDTO>();

            double score = 0;

            foreach (var question in questions)
            {
                var studentAnswer =
                    answers.FirstOrDefault(x => x.QuestionId == question.QuestionId);

                if (studentAnswer == null)
                    continue;

                var responseQuestion = new ResponseQuestionDTO
                {
                    QuestionId = question.QuestionId,
                    QuestionTitle = question.QuestionTitle,
                    QuestionType = question.QuestionType,
                    ImgLink = question.ImgLink != null
        ? _buunyStorageService.GenerateSecureUrl(question.ImgLink)
        : null,

                    Choices = question.Choices.Select(choice => new choicesDto
                    {
                        ChoiseId = choice.Id,
                        Text = choice.Text,
                        IsCorrect = choice.IsCorrect
                    }).ToList(),

                    choiceAnswerId = studentAnswer.ChoiceId,

                    IsCorrect = question.Choices
        .FirstOrDefault(x => x.Id == studentAnswer.ChoiceId)
        ?.IsCorrect ?? false
                };

                var selectedChoice =
                    question.Choices.FirstOrDefault(
                        x => x.Id == studentAnswer.ChoiceId);

                responseQuestion.IsCorrect =
                    selectedChoice?.IsCorrect ?? false;

                if (responseQuestion.IsCorrect)
                    score++;

                responseQuestions.Add(responseQuestion);
            }

            return new ReviewDTO
            {
                responseQuestionDTOs = responseQuestions,
                SubmittedAt = Attempt.SubmittedAt ?? DateTime.UtcNow,
                Score = score
            };
        }

    }
}
