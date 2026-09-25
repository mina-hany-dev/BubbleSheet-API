using bubblesheet.Infrastracture.Data;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Repos
{
    public class StudentAnswerRepo(bubblesheetDbContext context ,IUnitOfWork unitOfWork) : IStudentAnswer
    {
        private readonly bubblesheetDbContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task AddRangeAsync(List<StudentAnswer> answers)
        {
            await _context.studentAnswers.AddRangeAsync(answers);
        }
        public async Task RemoveRangeAsync(List<StudentAnswer> answers)
        {
            _context.studentAnswers.RemoveRange(answers);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<List<StudentAnswer>> GetAllAnswersByQuestionIDs(List<int> IDs)
        {
            return await _context.studentAnswers
                .Where(a => IDs.Contains(a.QuestionId))
                .ToListAsync();
        }
        public async Task<List<StudentAnswer>> GetAllAnswersByAttemptId(int Id)
        {
            return await _context.studentAnswers.Where(sa=>sa.StudentAttemptId == Id).ToListAsync();
        }
    }
}
