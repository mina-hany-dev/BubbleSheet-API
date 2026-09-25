using Domain.bublesheet.Entities;
using System.ComponentModel.DataAnnotations;

public class ExamQuestion
{
    [Key]
    public int Id { get; private set; }

    public int ExamId { get; private set; }
    public Exam Exam { get; private set; } = null!;

    public int QuestionId { get; private set; }
    public Question Question { get; private set; } = null!;

    private ExamQuestion() { }

    public ExamQuestion(int examId, int questionId)
    {
        ExamId = examId;
        QuestionId = questionId;
    }
}