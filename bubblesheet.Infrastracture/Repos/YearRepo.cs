using bubblesheet.Infrastracture.Data;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class YearRepo(bubblesheetDbContext context , IUnitOfWork unitOfWork) : IYear
    {
        private readonly bubblesheetDbContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<AcademicYear>> GetYearsByLevel(Levels level)
        {
            return await _context.academicYears.Where(y=>y.ACLevel == level).ToListAsync();
        }
        public async Task<AcademicYear> AddYearAsync(AcademicYear year)
        {
            await _context.AddAsync(year);
            await _unitOfWork.SaveChangesAsync();
            return year;
        }
        public async Task DeleteYear(int yearId)
        {
            var year = await _context.academicYears.FindAsync(yearId);

            if (year == null)
                return;

            _context.academicYears.Remove(year);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<AcademicYear> GetYearByLevel(Levels level)
        {
            return await _context.academicYears
                .FirstOrDefaultAsync(y => y.ACLevel == level);
        }

        public async Task<AcademicYear> GetLevelById(int YearId)
        {
            return await _context.academicYears
                .FirstOrDefaultAsync(y => y.AcademicYearId == YearId);
        }
        public async Task<AcademicYear> UpdateYear(AcademicYear year)
        {
            var existingYear = await _context.academicYears
                .FirstOrDefaultAsync(x => x.AcademicYearId == year.AcademicYearId);

            if (existingYear == null)
                return null;

            existingYear.Update(
                year.Name,
                year.imgLink
            );

            await _context.SaveChangesAsync();

            return existingYear;
        }
        public async Task<int?> GetAcademicYearIdByExamId(int examId)
        {
            return await _context.Exams
                .Where(e => e.ExamID == examId)
                .Select(e => e.Lesson.Subject.YearId)
                .FirstOrDefaultAsync();
        }
        public async Task<int?> GetAcademicYearIdByQBankId(int QbankId)
        {
            return await _context.QuestionBanks
                .Where(e => e.BankId == QbankId)
                .Select(e => e.Lesson.Subject.YearId)
                .FirstOrDefaultAsync();
        }

    }
}
