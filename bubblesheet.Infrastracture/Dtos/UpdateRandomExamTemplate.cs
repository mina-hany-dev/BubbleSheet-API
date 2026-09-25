using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class UpdateRandomExamTemplate
    {
        public int Id { get;  set; }
        public int LessonId { get;  set; }
        public int QuestionCount { get;  set; }
        public TimeSpan Duration { get; set; }
    }
}
