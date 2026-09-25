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
    public class QuestionExamRepo : IQuestionExam
    {
        private readonly bubblesheetDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public QuestionExamRepo(bubblesheetDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> GetNumberOfQuestionsInExams(List<int> examIds)
        {
            return await _context.examQuestions
                .CountAsync(x => examIds.Contains(x.ExamId));
        }
        public async Task<List<int>> GetIDQuestionsByExamId(int examId)
        {
            return await _context.examQuestions
                .Where(qe => qe.ExamId == examId)
                .Select(qe => qe.QuestionId)
                .ToListAsync();
        }
        public async Task AddRangeAsync(List<int> ids, int examId)
        {
            var examQuestions = ids
                .Select(id => new ExamQuestion(examId, id))
                .ToList();

            await _context.examQuestions.AddRangeAsync(examQuestions);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int QuestionId, int? ExamId)
        {
            if (ExamId == null)
            {
                var examQuestions = await _context.examQuestions
                    .Where(x => x.QuestionId == QuestionId)
                    .ToListAsync();

                _context.examQuestions.RemoveRange(examQuestions);
            }
            else
            {
                var examQuestion = await _context.examQuestions
                    .FirstOrDefaultAsync(x =>
                        x.QuestionId == QuestionId &&
                        x.ExamId == ExamId.Value);

                if (examQuestion != null)
                {
                    _context.examQuestions.Remove(examQuestion);
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task DeleteQuestionsFromAllExams(List<int> QuestionIDs)
        {
            var examQuestions = await _context.examQuestions
                .Where(x => QuestionIDs.Contains(x.QuestionId))
                .ToListAsync();

            _context.examQuestions.RemoveRange(examQuestions);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAllQuestionFromExam(int ExamId)
        {
            var examQuestions = await _context.examQuestions
                .Where(x => x.ExamId == ExamId)
                .ToListAsync();

            _context.examQuestions.RemoveRange(examQuestions);

            await _context.SaveChangesAsync();
        }
    }
}
