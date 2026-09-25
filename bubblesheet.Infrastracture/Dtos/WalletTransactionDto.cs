using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class WalletTransactionDto
    {
        public decimal Amount { get; set; }

        // الرصيد قبل العملية
        public decimal BalanceBefore { get; set; }

        // الرصيد بعد العملية
        public decimal BalanceAfter { get; set; }

        public TransactionType TransactionType { get; set; }

        // رقم الكارت أو الأوردر المرتبط
        public string? Reference { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalPaid { get; set; }
        public decimal TotalCash { get; set; }
    }
}
