using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface ICode
    {
        Task AddRangeCodeAsync(List<Code> code);
        Task DeleteAsync(int id);

        Task<Code?> GetByIdAsync(int id);

        Task<Code?> GetByTextAsync(string codeText);

        Task<List<Code>> GetAllAsync();
        Task UpdateAsync(Code code);
    }
}
