using bubblesheet.Infrastracture.Dtos;
using bubblesheet.Infrastracture.Repos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class QuestionService(IQuestionBank questionBank,IQuestion question,IUnitOfWork unitOfWork,IChoices choices, IQuestionExam questionExam,IBunnyStorageService bunnyStorageService) : IQuestionService
    {
        private readonly IQuestion _question = question;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IQuestionExam _questionExam = questionExam;
        private readonly IQuestionBank _questionBank = questionBank;
        private readonly IChoices _choice = choices;
        private readonly IBunnyStorageService _bannyStorageService = bunnyStorageService;

        public async Task<QuestionDTO> AddQuestion(AddQuestionsDto addQuestionsDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                string? imgLink = null;

                if (addQuestionsDto.Img != null)
                {
                    var uploadedImg = await _bannyStorageService
                        .UploadImageAsync(addQuestionsDto.Img, "Questions");

                    imgLink = uploadedImg.Url;
                }

                var question = new Question(
                    addQuestionsDto.QuestionType,
                    addQuestionsDto.QBankID,
                    imgLink,
                    addQuestionsDto.QuestionTitle
                );

                await _question.AddAsync(question);

                var choices = new List<choicesDto>();
                foreach (var choiceDto in addQuestionsDto.Choices)
                {
                    var choice = new Choice
                    {
                        Text = choiceDto.Text,
                        IsCorrect = choiceDto.IsCorrect,
                        QuestionId = question.QuestionId
                    };

                    var addedChoice = await _choice.AddAsync(choice);
                    choices.Add(new choicesDto
                    {
                        ChoiseId = addedChoice.Id,
                        Text = addedChoice.Text,
                        IsCorrect = addedChoice.IsCorrect
                    });
                }

                await _unitOfWork.CommitAsync();
                var QBank = await _questionBank.GetQuestionById(addQuestionsDto.QBankID);
                QBank.IncressQCount();
                await _unitOfWork.SaveChangesAsync();

                return new QuestionDTO
                {
                    QuestionId = question.QuestionId,
                    QuestionTitle = question.QuestionTitle,
                    QuestionType = question.QuestionType,
                    ImgLink = question.ImgLink,
                    Choices = choices
                };
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task<List<QuestionDTO>> GetQuestionsByIds(List<int> IDs)
        {
            var questions = await _question.GetQuestionsByIds(IDs);

            return questions.Select( q => new QuestionDTO
            {
                QuestionId = q.QuestionId,
                QuestionTitle = q.QuestionTitle,
                QuestionType = q.QuestionType,
                ImgLink =  q.ImgLink == null ? "" : _bannyStorageService.GenerateSecureUrl(q.ImgLink),

                Choices = q.Choices.Select(c => new choicesDto
                {
                    ChoiseId = c.Id,
                    Text = c.Text,
                    IsCorrect = c.IsCorrect
                }).ToList()

            }).ToList();
        }
        public async Task GenerateQuestionsAsync(int lessonId, int questionCount , int examId)
        {
            var questionBankIds = await _questionBank.GetAllQuestionBankIdsByLessonId(lessonId);
           
            var questionIds = await _question.GetQuestioIdsByBankId(questionBankIds);

            if (questionIds.Count < questionCount)
                throw new Exception("Not enough questions available.");

            var randomIds = questionIds
                .OrderBy(x => Random.Shared.Next())
                .Take(questionCount)
                .ToList();

            await _questionExam.AddRangeAsync(randomIds,examId);

            //return await _question.GetQuestionsByIds(randomIds);
        }
        public async Task DeleteQuestion(int Id)
        {
            var currentQuestion = await _question.GetQuestionById(Id);

            if (currentQuestion == null)
                return;

            var choices = await _choice.GetByQuestionId(Id);

            await _questionExam.DeleteAsync(Id,null);

            foreach (var choice in choices)
            {
                currentQuestion.RemoveChoice(choice);
                await _choice.DeleteChoice(choice.Id);
            }
            if(currentQuestion.ImgLink != null)
                await _bannyStorageService.DeleteAsync(currentQuestion.ImgLink);
            await _question.DeleteAsync(Id);
            var QBank = await _questionBank.GetQuestionById(currentQuestion.QBankID);
            QBank.DecressQCount();
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteQuestions(List<int> IDs)
        {
            foreach(var id in IDs)
            {
                await DeleteQuestion(id);
            }
        }
    }
}
