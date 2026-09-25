using bubblesheet.Infrastracture.Data;
using bubblesheet.Infrastracture.Dtos;
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
    public class QuestionBankRepo(IUnitOfWork unitOfWork , bubblesheetDbContext context) : IQuestionBank
    {
        private readonly bubblesheetDbContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<QuestionBank> AddQuestionBank(QuestionBank questionBank)
        {
            await _context.QuestionBanks.AddAsync(questionBank); 
            await _unitOfWork.SaveChangesAsync();
            return questionBank;
        }
        public async Task<QuestionBank?> GetQuestionById(int questionBankId)
        {
            return await _context.QuestionBanks
                .Include(q => q.Questions)
                 .Include(x => x.Lesson)
                .FirstOrDefaultAsync(q => q.BankId == questionBankId);
        }
        public async Task<List<int>> GetQuestionIdsByQuestionBankId(int questionBankId)
        {
            return await _context.Questions
                .Where(x => x.QBankID == questionBankId)
                .Select(x => x.QuestionId)
                .ToListAsync();
        }
        public async Task DeleteQuestionBank(int Id)
        {
            var questionBank = await _context.QuestionBanks
                .FirstOrDefaultAsync(x => x.BankId == Id);

            if (questionBank == null)
                return;

            _context.QuestionBanks.Remove(questionBank);
            await _context.SaveChangesAsync();
        }

        public async Task<QuestionBank> updateAsync(QuestionBank questionBank)
        {
            var existingQuestionBank = await _context.QuestionBanks
                .FirstOrDefaultAsync(x => x.BankId == questionBank.BankId);

            if (existingQuestionBank == null)
                return null;

            existingQuestionBank.Update(
       questionBank.BankName,
       questionBank.IsFree,
       questionBank.Description
       );

            await _context.SaveChangesAsync();

            return existingQuestionBank;
        }
        public async Task<List<int>> GetAllQuestionBankIdsByLessonId(int lessonId)
        {
            return await _context.QuestionBanks
                .Where(e => e.LessonID == lessonId && e.IsActive)
                .Select(e => e.BankId)
                .ToListAsync();
        }
        public async Task<int?> GetLessonIdByQuestionBankId(int questionBankId)
        {
            return await _context.QuestionBanks
                .Where(x => x.BankId == questionBankId)
                .Select(x => x.LessonID)
                .FirstOrDefaultAsync();
        }
    }
}
