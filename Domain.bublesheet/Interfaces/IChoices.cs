using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IChoices
    {
        Task<Choice> AddAsync(Choice choice);
        Task DeleteChoice(int Id);
        Task<IEnumerable<Choice>> GetByQuestionId(int questionId);
    }
}
