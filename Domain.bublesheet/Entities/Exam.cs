using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.bublesheet.Entities
{
    public class Exam
    {
        [Key]
        public int ExamID { get; private set; }
        public string ExamName { get; private set; }
        public TimeSpan Duration { get; private set; }
        public string Description { get; private set; }
        public int? StudentId { get; private set; } // if this = Null --> SO this Exam From The Teacher ,else --> from the Student
        public Student? Student { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int LessonID { get; private set; }
        [ForeignKey(nameof(LessonID))]
        public Lesson Lesson { get; private set; }
        private readonly List<ExamQuestion> _examQuestions = new();

        public IReadOnlyCollection<ExamQuestion> ExamQuestions =>
            _examQuestions.AsReadOnly();
        private Exam() { }

        public Exam(
            string examName,
            TimeSpan duration,
            string description,
            int LessonId,
            int? StuId = null)
        {
            SetExamName(examName);
            SetDuration(duration);
            Description = description;
            StudentId = StuId;
            CreatedAt = DateTime.UtcNow;
            this.LessonID = LessonId;
        }

        public void SetExamName(string examName)
        {
            if (string.IsNullOrWhiteSpace(examName))
                throw new ArgumentException("Exam name is required");

            ExamName = examName;
        }
        public void Update(
    string examName,
    TimeSpan duration,
    string description)
        {
            SetExamName(examName);
            SetDuration(duration);
            Description = description;
        }
        public void SetDuration(TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
                throw new ArgumentException("Duration must be greater than zero");

            Duration = duration;
        }
    }
}