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
    public class ChoiceRepo(bubblesheetDbContext context):IChoices
    {
        private readonly bubblesheetDbContext _context = context;
        public async Task<IEnumerable<Choice>> GetByQuestionId(int questionId)
        {
            return await _context.Choices
                .Where(x => x.QuestionId == questionId)
                .ToListAsync();
        }
        public async Task<Choice> AddAsync(Choice choice)
        {
            await _context.Choices.AddAsync(choice);
            await _context.SaveChangesAsync();

            return choice;
        }

        public async Task DeleteChoice(int Id)
        {
            var choice = await _context.Choices
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (choice == null)
                return;

            _context.Choices.Remove(choice);

            await _context.SaveChangesAsync();
        }
    }
}
