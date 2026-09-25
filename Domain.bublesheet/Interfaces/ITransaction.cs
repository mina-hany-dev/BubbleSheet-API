using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface ITransaction
    {
        Task AddAsync(WalletTransactions transactions);
        Task<List<WalletTransactions>> GetByStudentIdAsync(int studentId);
        Task<decimal> GetTotalCash(int studentId);
        Task<decimal> GetTotalPaid(int studentId);


    }
}
