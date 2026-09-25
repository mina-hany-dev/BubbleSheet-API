using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class AddchoicesDto
    {
        public string Text { get;  set; }

        public bool IsCorrect { get;  set; }
    }
    public class choicesDto : AddchoicesDto
    {
        public int ChoiseId { get; set; }
    }
}
