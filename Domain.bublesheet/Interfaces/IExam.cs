using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IExam
    {
        Task<int> AddAsyncExam(Exam exam);
        Task DeleteAsyncExam(int id);
        Task<List<Exam>> GetAllAsyncExmas();
        Task<Exam> UpdateExam(Exam exam);
        Task<Exam> GetExamById(int id);
        Task<int> GetCountOfExamsForStudent(int studentId);
        Task<List<int>> GetAllExamIdsByLessonId(int lessonId);
        Task<int?> GetLessonIdByExamId(int examId);
    }
}
