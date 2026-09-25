using bubblesheet.Infrastracture.Dtos;

namespace BubleSheet.Services.Interfaces
{
    public interface ILessonService
    {
        Task<List<LessonDto>> GetLessonBySubjectIdAsync(int SubjectId);
        Task<LessonDetailsDto> GetLessonDetails(int LessonId);
        Task<LessonDto> AddLessonAsync(AddLessonDTO DTO);
        Task<LessonDto> EditLesson(EditLessonDto Dto);
        Task DeleteLesson(int LessonId);
        Task DeleteLessons(List<int> LessonIDs);
    }
}
