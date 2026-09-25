using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IStudentLesson
    {
        Task<Dictionary<int, int>> GetCompletedLessonsCountBySubjectIds(
    int studentId,
    List<int> subjectIds);
        Task<int> GetCompletedLessonsCountBySubjectId(
    int studentId,
    int subjectId);

        Task PutLessonsinStudentLessons(int studentId, List<int> lessonsIds);
        Task<List<StudentLesson>> GetAllStudentLessons();
        Task<List<int>> GetExistLessonsIds(int studentId, List<int> lessonIds);
        Task<StudentLesson?> GetStudentLessonByStudentIdAndLessonId(
    int studentId,
    int lessonId);
        Task AddRangeAsync(List<StudentLesson> studentLessons);
        Task DeleteAllStudentLessonsByessonId(int Id);
    }
}
