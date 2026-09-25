using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IYear
    {
        Task<List<AcademicYear>> GetYearsByLevel(Levels level);
        Task<AcademicYear> AddYearAsync(AcademicYear year);
        Task DeleteYear(int yearId);
        Task<AcademicYear> GetYearByLevel (Levels level);
        Task<AcademicYear> GetLevelById (int YearId);
        Task<AcademicYear> UpdateYear(AcademicYear year);
        Task<int?> GetAcademicYearIdByExamId(int examId);
        Task<int?> GetAcademicYearIdByQBankId(int QbankId);

    }
}
