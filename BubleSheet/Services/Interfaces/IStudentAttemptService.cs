using bubblesheet.Infrastracture.Dtos;
using Domain.bublesheet.Entities;

namespace BubleSheet.Services.Interfaces
{
    public interface IStudentAttemptService
    {
        Task<int> AddStudentAttemptAsync(StudentAttemptDTO attemptDTO);
        Task<bool> IsStudentCompeleteAllLesson(
     int studentId,
     int lessonId);
    }
}
