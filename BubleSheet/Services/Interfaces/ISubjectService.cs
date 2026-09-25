using bubblesheet.Infrastracture.Dtos;

namespace BubleSheet.Services.Interfaces
{
    public interface ISubjectService
    {
        Task BuySubject(BuySubjectDto buySubjectDto);
        Task<List<SubjectsDto>> GetSubjectsByYearId(int yearId);
        Task<List<DataofSubjectDto>> GetSubjectsLessonDataByYearId(int yearId);
        Task<SubjectDetailsDto> GetSubjectDetails(int subjectId);
        Task<SubjectDto> AddSubjectAsync (AddSubjectDto subjectDto);
        Task<SubjectDto> EditSubjectAsync(EditSubjectDTO editSubjectDTO);
        Task DeleteSubject(int SubjectId);
        Task DeleteSubjects(List<int> IDs);
    }
}
