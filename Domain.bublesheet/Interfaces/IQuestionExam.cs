using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IQuestionExam
    {
        Task<List<int>> GetIDQuestionsByExamId(int ExamId);
        Task AddRangeAsync(List<int> ids, int examId);
        Task DeleteAsync(int QuestionId, int? ExamId);// if ExamId = null --> Delete from All Exams , else Delete from the Exam Only
        Task DeleteQuestionsFromAllExams(List<int> QuestionIDs);
        Task DeleteAllQuestionFromExam(int ExamId);
        Task<int> GetNumberOfQuestionsInExams(List<int> ExamIDs);
    }
}
