using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class CodeDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsUsed { get; set; }
        public decimal Amount { get; set; }
    }
    public class AddCodeDto
    {
        public decimal balance { get; set;}
        public int count { get; set; } = 1;
    }
}
