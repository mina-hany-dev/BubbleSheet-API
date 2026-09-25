using bubblesheet.Infrastracture.Data;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Repos
{
    public class StudentScoreRepo(bubblesheetDbContext dbContext) : IStudentScore
    {
        private readonly bubblesheetDbContext _context = dbContext;


        public async Task<StudentScore?> GetByStudentAndYearAsync(
            int studentId,
            int acYearId)
        {
            return await _context.StudentScores
                .FirstOrDefaultAsync(x =>
                    x.StudentId == studentId &&
                    x.AcYearId == acYearId);
        }
        public async Task<IEnumerable<StudentScore>> GetByAcYearIdAsync(int acYearId)
        {
            return await _context.StudentScores
                .Where(x => x.AcYearId == acYearId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentScore>> GetByStudentIdAsync(
            int studentId)
        {
            return await _context.StudentScores
                .Where(x => x.StudentId == studentId)
                .ToListAsync();
        }

        public async Task AddAsync(StudentScore studentScore)
        {
            await _context.StudentScores.AddAsync(studentScore);
        }

        public void Update(StudentScore studentScore)
        {
            _context.StudentScores.Update(studentScore);
        }

        public void Delete(StudentScore studentScore)
        {
            _context.StudentScores.Remove(studentScore);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    
    }
}
