using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class QuestionBankDTO
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
        public int QCount { get; set; }
        public bool IsFree { get; set; }
        public string Description { get; set; }
        public bool HasSolved {  get; set; }
        public bool IsActive { get; set; }
    }
    public class EditQuestionBankDTO
    {
        public int QId {  get; set; }
        public string BankName { get; set; }
        public bool IsFree { get; set; }
        public string Description { get; set; }
    }
    public class AddQuestionBankDTO
    {
        public int LessonId { get; set; }
        public string BankName { get; set; }
        public bool IsFree { get; set; }
        public string Description { get; set; }
    }
    public class QuestionBankQuestionDTO : QuestionBankDTO
    {
        public List<QuestionDTO> Questions { get; set; }
    }
    public class QuestionBankQuestionWithAttemptID : QuestionBankQuestionDTO
    {
        public int AttemptId { get; set; }
    }
}
