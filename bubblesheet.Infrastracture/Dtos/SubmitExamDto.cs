using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class SubmitDto
    {
        public int StudentAttemptId { get; set; }
        public List<AnswerDto> Answers { get; set; } = new();
    }
    public class SumbitResponseDto
    {
        public int Score { get; set; }
        public int precentage { get; set; }
    }
}
