using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.bublesheet.Entities
{
    public class QuestionBank
    {
        [Key]
        public int BankId { get; private set; }
        public string BankName { get; private set; }
        public int QCount { get; private set; }
        public string Description { get; private set; }
        public bool IsFree { get; private set; }
        public bool IsActive { get; private set; }
        public int LessonID { get; private set; }
        [ForeignKey(nameof(LessonID))]
        public Lesson Lesson { get; private set; }
        private readonly List<Question> _questions = new();
        public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();
        private QuestionBank() { }
        public QuestionBank(string bankName, bool isFree,string desc,int LessonId)
        {
            SetBankName(bankName);
            IsFree = isFree;
            LessonID = LessonId;
            Description = desc;
            QCount = 0;
            IsActive = true;
        }

        public void UpdateBankName(string bankName)
        {
            SetBankName(bankName);
        }

        private void SetBankName(string bankName)
        {
            if (string.IsNullOrWhiteSpace(bankName))
                throw new ArgumentException("Bank name is required.");

            BankName = bankName.Trim();
        }
        public void MakeToggle()
        {
            IsActive = !IsActive;
        }
        public void MakeFree()
        {
            IsFree = true;
        }
        public void MakePaid(double price)
        {
            IsFree = false;
        }
        public void IncressQCount()
        {
            QCount += 1;
        }
        public void DecressQCount()
        {
            QCount -= 1;
        }
        public void Update(
    string bankName,
    bool isFree,
    string description)
        {
            SetBankName(bankName);

            IsFree = isFree;
            Description = description;
        }
        public int GetQuestionsCount()
        {
            return _questions.Count;
        }

        public bool HasQuestions()
        {
            return _questions.Any();
        }
    }
}