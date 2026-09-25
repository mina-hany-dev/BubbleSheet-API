using bubblesheet.Infrastracture.Data;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Repos
{
    public class StudentAttemptRepo(bubblesheetDbContext context , IUnitOfWork unitOfWork) : IStudentAttempts
    {
        private readonly bubblesheetDbContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> HasStudentTakenFreeAttemptExam(int studentId, int examId)
        {
            return await _context.studentAttempts
                .AnyAsync(x =>
                    x.StudentId == studentId &&
                    x.ExamId == examId
                    && x.SubmitType == Domain.bublesheet.Entities.enums.submitType.Exam
                    && x.IsSubmitted);
        }
        public async Task<List<int?>> GetAllStudentAttemptExamIds(int studentId)
        {
            return await _context.studentAttempts
                .Where(x => x.StudentId == studentId && x.IsSubmitted)
                .Select(x => x.ExamId)
                .ToListAsync();
        }
        public async Task<List<int?>> GetAllStudentAttemptQuestionBankIds(int studentId)
        {
            return await _context.studentAttempts
                .Where(x => x.StudentId == studentId && x.IsSubmitted)
                .Select(x => x.QuestionBankId)
                .ToListAsync();
        }
        public async Task RemoveStudentAttempt(int attemptId)
        {
            var attempt = await _context.studentAttempts.FindAsync(attemptId);

            if (attempt == null)
                return;

            _context.studentAttempts.Remove(attempt);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<int?> GetAttemptIdByIdAndType(
     int studentId,
     int id,
     submitType submitType)
        {
            var query = _context.studentAttempts
                .Where(a =>
                    a.StudentId == studentId &&
                    a.IsSubmitted &&
                    a.SubmitType == submitType &&
                    (a.ExamId == id || a.QuestionBankId == id));

            if (submitType == submitType.QuestionBank)
            {
                return await query
                    .OrderByDescending(a => a.SubmittedAt)
                    .Select(a => (int?)a.StudentAttemptId)
                    .FirstOrDefaultAsync();
            }

            return await query
                .Select(a => (int?)a.StudentAttemptId)
                .FirstOrDefaultAsync();
        }
        public async Task AddAsync(StudentAttempt studentAttempt)
        {
            await _context.studentAttempts.AddAsync(studentAttempt);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<StudentAttempt?> GetById(int attemptId)
        {
            return await _context.studentAttempts
                .FirstOrDefaultAsync(x => x.StudentAttemptId == attemptId);
        }
        public async Task<StudentAttempt?> GetAttemptHasNotSumbittedByIdForExam(
            int studentId,
            int examId)
        {
            return await _context.studentAttempts
                .FirstOrDefaultAsync(x =>
                    x.StudentId == studentId &&
                    x.ExamId == examId &&
                    !x.IsSubmitted);
        }

        public async Task<StudentAttempt?> GetAttemptHasNotSumbittedByIdForQuestionBank(
            int studentId,
            int questionBankId)
        {
            return await _context.studentAttempts
                .FirstOrDefaultAsync(x =>
                    x.StudentId == studentId &&
                    x.QuestionBankId == questionBankId &&
                    !x.IsSubmitted);
        }
        public async Task<List<StudentAttempt>> GetStudentAttempetsByStudentId(int studentId)
        {
            return await _context.studentAttempts.Where(sa=>sa.StudentId  == studentId && sa.IsSubmitted).ToListAsync();
        }
        public async Task<List<int>> GetAllAttemptIDs(
    int id,
    submitType submitType,
    bool all = false)
        {
            return await _context.studentAttempts
                .Where(a =>
                    (all || a.IsSubmitted) &&
                    a.SubmitType == submitType &&
                    (a.ExamId == id || a.QuestionBankId == id))
                .Select(a => a.StudentAttemptId)
                .ToListAsync();
        }
        public async Task RemoveRangeStudentAttempt(List<int> attemptIds)
        {
            var attempts = await _context.studentAttempts
                .Where(x => attemptIds.Contains(x.StudentAttemptId))
                .ToListAsync();

            if (attempts.Count == 0)
                return;

            _context.studentAttempts.RemoveRange(attempts);
        }
        public async Task<int> GetNumberOfAttemptByStudentIdAndType(int StudentId,submitType submitType)
        {
            return await _context.studentAttempts
        .CountAsync(x =>
            x.IsSubmitted &&
            x.StudentId == StudentId &&
            x.SubmitType == submitType);
        }
        public async Task<bool> IfStudentSolveQbank(int studentId, int qBank)
        {
            return await _context.studentAttempts
                .AnyAsync(x => x.SubmitType == submitType.QuestionBank && x.StudentId == studentId && x.QuestionBankId == qBank && x.IsSubmitted && x.IsFirst);
        }
    }
}
