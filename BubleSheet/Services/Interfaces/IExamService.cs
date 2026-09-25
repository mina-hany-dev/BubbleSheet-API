using bubblesheet.Infrastracture.Dtos;

namespace BubleSheet.Services.Interfaces
{
    public interface IExamService
    {
        Task<int> AddExamAsync(AddExamDto addExamDto, bool saveChanges = true);
        Task<ExamDto> EditExam(EditExamDto Dto);
        Task DeleteExam(int id);
        Task DeleteExams(List<int> IDs);
        Task<ExamQuestionWithAttemptID> GetExamWithQuestions(int ExamId);
        Task<ExamDto> GetExam(int ExamId);
        Task<int> CreateAnRandomExam(int LessonId);
        Task<SumbitResponseDto> SubmitExam(SubmitDto submitDto);
        Task UpdateExamQuestions(UpdateExamQuestionsDto dto);
    }
}
