using Domain.bublesheet.Entities.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.bublesheet.Entities
{
    public class StudentAttempt
    {
        [Key]
        public int StudentAttemptId { get; private set; }

        public int StudentId { get; private set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; private set; }

        public submitType SubmitType { get; private set; }

        public int? ExamId { get; private set; }

        [ForeignKey(nameof(ExamId))]
        public Exam? Exam { get; private set; }

        public int? QuestionBankId { get; private set; }

        [ForeignKey(nameof(QuestionBankId))]
        public QuestionBank? QuestionBank { get; private set; }

        public DateTime StartAt { get; private set; }

        public DateTime? SubmittedAt { get; private set; }

        public double Score { get; private set; }

        public bool IsSubmitted { get; private set; }
        public bool IsFirst {  get; private set; }

        private StudentAttempt() { }

        public static StudentAttempt CreateExamAttempt(int studentId, int examId)
        {
            return new StudentAttempt
            {
                StudentId = studentId,
                ExamId = examId,
                SubmitType = submitType.Exam,
                StartAt = DateTime.UtcNow,
                IsFirst = false
            };
        }

        public static StudentAttempt CreateQuestionBankAttempt(int studentId, int questionBankId, bool IsFirst = false)
        {
            return new StudentAttempt
            {
                StudentId = studentId,
                QuestionBankId = questionBankId,
                SubmitType = submitType.QuestionBank,
                StartAt = DateTime.UtcNow,
                IsFirst = IsFirst
            };
        }
        public void ToggleIsFirst()
        {
            IsFirst = true;
        }

        public void Submit(double score)
        {
            if (IsSubmitted)
                throw new InvalidOperationException("This attempt has already been submitted.");

            Score = score;
            SubmittedAt = DateTime.UtcNow;
            IsSubmitted = true;
        }
    }
}