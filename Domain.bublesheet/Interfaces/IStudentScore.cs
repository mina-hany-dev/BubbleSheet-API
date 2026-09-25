using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IStudentScore
    {
        Task<StudentScore?> GetByStudentAndYearAsync(
            int studentId,
            int acYearId);

        Task<IEnumerable<StudentScore>> GetByStudentIdAsync(
            int studentId);
        Task<IEnumerable<StudentScore>> GetByAcYearIdAsync(
            int AcYearId);

        Task AddAsync(StudentScore studentScore);

        void Update(StudentScore studentScore);

        void Delete(StudentScore studentScore);

        Task SaveChangesAsync();
    }
}
