using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.bublesheet.Entities.enums;

namespace bubblesheet.Infrastracture.Dtos
{
    public class StudentAttemptDTO
    {
        public int Id { get; set; }
        public submitType submitType { get; set; }
    }
}
