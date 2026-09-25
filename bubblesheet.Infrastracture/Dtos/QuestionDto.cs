using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class AddQuestionsDto
    {
        public string? QuestionTitle { get; set; }

        public QuestionType QuestionType { get; set; }
        public IFormFile? Img { get; set; }

        public List<AddchoicesDto> Choices { get; set; } = new();
        public int QBankID { get;  set; }
    }
    public class QuestionDTO
    {
        public int QuestionId { get; set; }
        public string? QuestionTitle { get; set; }
        public QuestionType QuestionType { get; set; }
        public string? ImgLink { get; set; }
        public List<choicesDto> Choices { get; set; } = new();
    }
    public class ResponseQuestionDTO : QuestionDTO
    {
        public int? choiceAnswerId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
