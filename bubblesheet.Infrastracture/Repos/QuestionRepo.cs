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
    public class QuestionRepo(bubblesheetDbContext context) : IQuestion
    {
        private readonly bubblesheetDbContext _context = context;
        public async Task AddAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var Question = await _context.Questions.FirstOrDefaultAsync(q=>q.QuestionId == id);
            if (Question != null)
            {
                _context.Questions.Remove(Question);
                await _context.SaveChangesAsync();
            }
            new Exception("Invalid Question");
        }
        public async Task<List<Question>> GetQuestionsByIds(List<int> IDs)
        {
            return await _context.Questions
       .Where(q => IDs.Contains(q.QuestionId))
       .Include(q => q.Choices)
       .ToListAsync();
        }
        public async Task<int?> GetCountOfQuestions()
        {
            return await _context.Questions.CountAsync();
        }
        public async Task<List<int>> GetQuestioIdsByBankId(List<int> qBankIds)
        {
            return await _context.Questions
                .Where(q => qBankIds.Contains(q.QBankID))
                .Select(q => q.QuestionId)
                .ToListAsync();
        }
        public async Task<Question> GetQuestionById(int ID)
        {
            return await _context.Questions.FirstOrDefaultAsync(q => q.QuestionId == ID);
        }
    }
}
