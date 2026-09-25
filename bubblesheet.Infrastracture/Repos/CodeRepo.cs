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
    public class CodeRepo : ICode
    {
        private readonly bubblesheetDbContext _context;

        public CodeRepo(bubblesheetDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeCodeAsync(List<Code> code)
        {
            if (code == null)
                throw new ArgumentNullException(nameof(code));

            await _context.Codes.AddRangeAsync(code);
            await Task.CompletedTask;

        }

        public async Task DeleteAsync(int id)
        {
            var code = await _context.Codes.FirstOrDefaultAsync(x => x.CodeId == id);

            if (code != null)
                _context.Codes.Remove(code);
            await Task.CompletedTask;
        }

        public async Task<Code?> GetByIdAsync(int id)
        {
            return await _context.Codes
                .FirstOrDefaultAsync(x => x.CodeId == id);
        }

        public async Task<Code?> GetByTextAsync(string codeText)
        {
            if (string.IsNullOrWhiteSpace(codeText))
                return null;

            return await _context.Codes
                .FirstOrDefaultAsync(x => x.CodeText == codeText);
        }

        public async Task<List<Code>> GetAllAsync()
        {
            return await _context.Codes.ToListAsync();
        }
        public async Task UpdateAsync(Code code)
        {
            _context.Codes.Update(code);
            await Task.CompletedTask;
        }
    }
}
