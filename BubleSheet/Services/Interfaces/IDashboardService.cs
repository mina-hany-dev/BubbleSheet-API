using bubblesheet.Infrastracture.Dtos;
using Domain.bublesheet.Entities.enums;

namespace BubleSheet.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DataResponseDto> GetMainDataforStudent(int stuId);
        Task<List<YearsResponseDto>> GetAllYearsByLevel (Levels level);
        Task<List<SubjectsDto>> GetAllSubjects(int id);
        Task<AdminDataResponseDto> GetMainDataforAdmin(Levels level = Levels.level2);
        Task<List<StudentTableDto>> GetTableStudent(int AcYearId, bool IsStudent);
        Task<(int, int, double)> GetStudentSolvedData();

    }
}
