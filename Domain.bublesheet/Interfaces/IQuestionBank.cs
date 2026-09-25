using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IQuestionBank
    {
        Task <QuestionBank>AddQuestionBank(QuestionBank questionBank);
        Task DeleteQuestionBank(int Id);
        Task<QuestionBank> updateAsync(QuestionBank questionBank);
        Task<QuestionBank?> GetQuestionById (int QuestionBankId);
        Task<List<int>>GetQuestionIdsByQuestionBankId(int questionBankId);
        Task<List<int>> GetAllQuestionBankIdsByLessonId(int lessonId);
        Task<int?> GetLessonIdByQuestionBankId(int questionBankId);
    }
}
