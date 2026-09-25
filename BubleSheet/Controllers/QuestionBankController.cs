using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Implementation;
using BubleSheet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubleSheet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionBankController(IReviewService reviewService,IQuestionBankService questionBankService) : ControllerBase
    {
        private readonly IQuestionBankService _questionBankService = questionBankService;
        private readonly IReviewService _reviewService = reviewService;
        [Authorize]
        [HttpGet("get-QuestionBank")] //GetDetails
        public async Task<IActionResult> GetQuestionBank(int QBankId)
        {
            var Exam = await _questionBankService.GetQuestionBank(QBankId);
            return Ok(Exam);
        }

        [Authorize]
        [HttpGet("open-QuestionBank")] // Get Details + Questions
        public async Task<IActionResult> OpenQuestionBank(int QBankId)
        {
            try
            {
                var exam = await _questionBankService
                    .GetQuestionBankWithQuestions(QBankId);

                return Ok(exam);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while opening the question bank.",
                    error = ex.Message
                });
            }
        }
        [Authorize]
        [HttpPost("submit-QuestionBank")]
        public async Task<IActionResult> SubmitQuestionBank(SubmitDto submitDto)
        {
            var sumbit = await _questionBankService.SubmitQuestionBank(submitDto);
            return Ok(sumbit);
        }
        [Authorize]
        [HttpGet("get-qbank-answers")]
        public async Task<IActionResult> GetQuestionBankAnswers(int QuestionBankId)
        {
            return Ok();
        }
        [Authorize]
        [HttpGet("get-review")]
        public async Task<IActionResult> Review(int Id)
        {
            try
            {
                var response = await _reviewService.ReviewQuestions(
                    Id,
                    Domain.bublesheet.Entities.enums.submitType.QuestionBank);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
        //Admin Actions
        [Authorize(Roles ="Admin")]
        [HttpPost("add-questionBank")]
        public async Task<IActionResult> AddQuestionBank([FromForm]AddQuestionBankDTO Dto)
        {
            var newQuestionBank = await _questionBankService.AddQuestionBank(Dto);
            return Ok(newQuestionBank);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("edit-QuestionBank")]
        public async Task<IActionResult> EditQuestionBank([FromForm] EditQuestionBankDTO Dto)
        {
            var newQuestionBank = await _questionBankService.EditQuestionBank(Dto);
            return Ok(newQuestionBank);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-questionBank")]
        public async Task<IActionResult> DeleteQuestionBank(int Id)
        {
            await _questionBankService.DeleteQuestionBank(Id);
            return Ok("Delete is Done");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("make-toggle")]
        public async Task<IActionResult> MakeToggle(int Id)
        {
            await _questionBankService.MakeToggle(Id);
            return Ok();
        }

    }
}
