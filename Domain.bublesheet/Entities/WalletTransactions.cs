using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class WalletTransactions
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student student { get; set; }
        // قيمة الحركة
        public decimal Amount { get; set; }

        // الرصيد قبل العملية
        public decimal BalanceBefore { get; set; }

        // الرصيد بعد العملية
        public decimal BalanceAfter { get; set; }

        public TransactionType TransactionType { get; set; }

        // رقم الكارت أو الأوردر المرتبط
        public string? Reference { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public void CreateRecharge(
           decimal amount,
           decimal currentBalance,
           string? rechargeCard = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Recharge amount must be greater than zero.");

            Amount = amount;
            BalanceBefore = currentBalance;
            BalanceAfter = currentBalance + amount;
            TransactionType = TransactionType.RechargeCard;
            Reference = rechargeCard;
        }

        public void CreateCoursePurchase(
            decimal amount,
            decimal currentBalance)
        {
            if (amount <= 0)
                throw new ArgumentException("Purchase amount must be greater than zero.");

            if (currentBalance < amount)
                throw new InvalidOperationException("Insufficient balance.");

            Amount = amount;
            BalanceBefore = currentBalance;
            BalanceAfter = currentBalance - amount;
            TransactionType = TransactionType.CoursePurchase;
        }
    }
}
