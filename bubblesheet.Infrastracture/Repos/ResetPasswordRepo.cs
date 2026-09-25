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
    public class ResetPasswordRepo : IResetPassword
    {
        private readonly bubblesheetDbContext _context;

        public ResetPasswordRepo(bubblesheetDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddAsync(ResetPassword resetPassword)
        {
            await _context.resetPasswords.AddAsync(resetPassword);
            await _context.SaveChangesAsync();

            return resetPassword.CodeOTP;
        }

        public async Task RemoveAsync(int userId)
        {
            var resetPassword = await _context.resetPasswords
                .FirstOrDefaultAsync(x => x.StudentId == userId);

            if (resetPassword == null)
                return;

            _context.resetPasswords.Remove(resetPassword);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ResetPassword resetPassword)
        {
            _context.resetPasswords.Update(resetPassword);
            await _context.SaveChangesAsync();
        }
        public async Task<ResetPassword?> GetAsync(int userId)
        {
            return await _context.resetPasswords
                .FirstOrDefaultAsync(x => x.StudentId == userId);
        }
    }
}
