using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class ReviewDTO
    {
        public List<ResponseQuestionDTO> responseQuestionDTOs { get; set; } = new();
        public DateTime SubmittedAt { get;  set; }
        public double Score { get; set; }
    }
}
