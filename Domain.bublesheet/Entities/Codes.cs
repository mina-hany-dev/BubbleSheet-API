using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class Code
    {
        [Key]
        public int CodeId { get; private set; }

        public string CodeText { get; private set; }
        public bool Sold { get; private set; }

        public decimal Amount { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Code() { }

        public Code(string codeText, decimal balance)
        {
            SetCodeText(codeText);
            Amount = balance;
            Sold = false;
            CreatedAt = DateTime.UtcNow;
        }

        private void SetCodeText(string codeText)
        {
            if (string.IsNullOrWhiteSpace(codeText))
                throw new ArgumentException("Code text is required");

            CodeText = codeText.Trim();
        }

        public void MarkAsSold()
        {
            if (Sold)
                throw new InvalidOperationException("Code already sold");

            Sold = true;
        }

    }
}
