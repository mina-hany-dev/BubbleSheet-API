using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IStudentAnswer
    {
        Task AddRangeAsync(List<StudentAnswer> answers);
        Task RemoveRangeAsync(List<StudentAnswer> answers);
        Task<List<StudentAnswer>> GetAllAnswersByQuestionIDs(List<int> IDs);
        Task <List<StudentAnswer>> GetAllAnswersByAttemptId(int Id);
    }
}
