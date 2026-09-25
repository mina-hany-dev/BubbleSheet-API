using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IQuestion
    {
        Task AddAsync(Question question);
        Task DeleteAsync(int id);
        Task<List<Question>> GetQuestionsByIds(List<int> IDs);
        Task<List<int>> GetQuestioIdsByBankId(List<int> qBankIds);
        Task<Question> GetQuestionById(int ID);
        Task<int?> GetCountOfQuestions();
    }
}
