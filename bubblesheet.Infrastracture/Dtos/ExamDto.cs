using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class AddExamDto
    {
        public string ExamName { get; set; }
        public TimeSpan Duration { get; set; }
        public int LessonId { get; set; }
        public string Description { get; set; }
        public int? StudentId { get; set; }
    }

    public class EditExamDto
    {
        public string ExamName { get; set; }
        public TimeSpan Duration { get; set; }
        public string Description { get; set; }
        public int ExamId { get; set; }
    }
    public class ExamDto
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public int QCount { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public bool HasOneFree {  get; set; }
    }
    public class ExamQuestionDTO : ExamDto
    {
        public List<QuestionDTO> Questions { get; set; }
    }
    public class ExamQuestionWithAttemptID : ExamQuestionDTO
    {
        public int AttemptId { get; set; }
    }
    public class UpdateExamQuestionsDto
    {
        public int ExamId { get; set; }

        public int LessonId { get; set; }

        public List<int> QuestionIds { get; set; } = new();
    }
}
