using bubblesheet.Infrastracture.Dtos;

namespace BubleSheet.Services.Interfaces
{
    public interface IQuestionBankService
    {
        Task<QuestionBankQuestionWithAttemptID> GetQuestionBankWithQuestions(int questionBankId);
        Task<QuestionBankDTO> GetQuestionBank(int QuestionBankId);
        Task<SumbitResponseDto> SubmitQuestionBank(SubmitDto submitDto);
        Task<QuestionBankDTO> AddQuestionBank(AddQuestionBankDTO questionBankDTO);
        Task<QuestionBankDTO> EditQuestionBank(EditQuestionBankDTO questionBankDTO);
        Task DeleteQuestionBank(int Id);
        Task DeleteQuestionBanks(List<int> IDs);
        Task MakeToggle(int QuestionBankId);
    }
}
