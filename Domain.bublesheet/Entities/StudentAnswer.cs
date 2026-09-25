using Domain.bublesheet.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class StudentAnswer
{
    [Key]
    public int Id { get; private set; }

    public int StudentAttemptId { get; private set; }

    [ForeignKey(nameof(StudentAttemptId))]
    public StudentAttempt StudentAttempt { get; private set; }

    public int QuestionId { get; private set; }

    [ForeignKey(nameof(QuestionId))]
    public Question Question { get; private set; }

    public int? ChoiceId { get; private set; }

    [ForeignKey(nameof(ChoiceId))]
    public Choice Choice { get; private set; }

    public bool IsCorrect { get; private set; }


    private StudentAnswer() { }

    public StudentAnswer(
        int studentAttemptId,
        int questionId,
        int? choiceId,
        bool isCorrect)
    {
        if(choiceId == null)
            IsCorrect = false;
        StudentAttemptId = studentAttemptId;
        QuestionId = questionId;
        ChoiceId = choiceId;
        IsCorrect = isCorrect;
    }
}