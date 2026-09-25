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
    public class TranscationRepo(bubblesheetDbContext context) : ITransaction
    {
        private readonly bubblesheetDbContext _context = context;
        public async Task AddAsync(WalletTransactions transactions)
        {
            await _context.WalletTransactions.AddAsync(transactions);
            await Task.CompletedTask;
        }
        public async Task<List<WalletTransactions>> GetByStudentIdAsync(int studentId)
        {
            return await _context.WalletTransactions
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        public async Task<decimal> GetTotalCash(int studentId)
        {
            return await _context.WalletTransactions
                .Where(t => t.StudentId == studentId &&
                            t.TransactionType == TransactionType.RechargeCard)
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetTotalPaid(int studentId)
        {
            return await _context.WalletTransactions
                .Where(t => t.StudentId == studentId &&
                            t.TransactionType == TransactionType.CoursePurchase)
                .SumAsync(t => t.Amount);
        }
    }
}
