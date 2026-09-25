using bubblesheet.Infrastracture.Dtos;
using bubblesheet.Infrastracture.Repos;
using Domain.bublesheet.Entities;

namespace BubleSheet.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionDTO> AddQuestion(AddQuestionsDto addQuestionsDto);
        Task<List<QuestionDTO>> GetQuestionsByIds(List<int> IDs);
        Task GenerateQuestionsAsync(int lessonId, int questionCount,int ExamId);
        Task DeleteQuestion(int Id);
        Task DeleteQuestions(List<int> IDs);

    }
}
