using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class LessonDto
    {
        public int LessonID { get;  set; }

        public string LessonName { get;  set; }
        public string? Description { get;  set; }
        public int Index { get; set; }
        public LessonStatue LessonStatue { get; set; }
    }
    public class LessonDetailsDto : LessonDto
    {
        public string subjectName { get; set; }
        public bool Paid { get; set; }
        public decimal Price { get; set; }
        public List<ExamDto> Exams { get; set; } = new();
        public List<PDFDTO> PDFs {get;set;} = new();
        public List<QuestionBankDTO> questionBanks { get; set; } = new();
    }
    public class AddLessonDTO
    {
        public int SubjectId { get; set; }
        public string Name  { get; set; }
        public string? Description { get; set; }
    }
    public class EditLessonDto
    {
        public int LessonId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
