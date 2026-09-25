using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class Question
    {
        [Key]
        public int QuestionId { get; private set; }

        public string? QuestionTitle { get; private set; }

        public QuestionType QuestionType { get; private set; }
        public string? ImgLink { get; private set; } 

        private readonly List<Choice> _choices = new();
        public int QBankID {  get; private set; }
        [ForeignKey(nameof(QBankID))]
        public QuestionBank question { get; private set; }
        private readonly List<ExamQuestion> _examQuestions = new();

        public IReadOnlyCollection<ExamQuestion> ExamQuestions =>
            _examQuestions.AsReadOnly();

        public IReadOnlyCollection<Choice> Choices => _choices.AsReadOnly();

        private Question() { }

        public Question(
            QuestionType questionType,
            int QBankid,
            string? ImgLink = null,
            string?questionTitle = null)
        {
            QuestionType = questionType;
            QBankID = QBankid;
            if (ImgLink !=null)
                this.ImgLink = ImgLink;
            if(questionTitle != null)
                this.QuestionTitle = questionTitle;
        }

        public void AddChoice(Choice choice)
        {
            if (choice == null)
                throw new ArgumentNullException(nameof(choice));

            _choices.Add(choice);
        }

        public void RemoveChoice(Choice choice)
        {
            if (choice == null)
                throw new ArgumentNullException(nameof(choice));

            _choices.Remove(choice);
        }
    }
}
