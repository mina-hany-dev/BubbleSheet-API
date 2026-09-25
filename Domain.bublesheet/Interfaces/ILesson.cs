using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface ILesson
    {
        Task<List<Lesson>> GetLessonsBySubjectId(int SubjectId);
        Task<int> GetLessonCountBySubjectId(int subjectId);
        Task<Dictionary<int, int>> GetLessonCountsBySubjectIds(List<int> subjectIds);
        Task<Lesson> GetLessonDetailsById(int lessonId);
        Task<Lesson> UpdateLesson(Lesson lesson);
        Task DeleteLesson(int Id);
        Task<Lesson> AddAsync(Lesson lesson);
        Task<int> LastIndex(int SubjectId);
    }
}
