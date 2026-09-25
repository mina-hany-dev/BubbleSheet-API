using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubleSheet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExamController(IReviewService reviewService,IExamService examService, ILogger<ExamController> logger) : ControllerBase
    {
        private readonly IExamService _examService = examService;
        private readonly ILogger<ExamController> _logger = logger;
        private readonly IReviewService _reviewService = reviewService;

        //Student Actions
        [Authorize]
        [HttpPost("Create-Random-Exam")]
        public async Task<IActionResult> CreateRandomExam(int LessonId)
        {
            try
            {
                var newExam = await _examService.CreateAnRandomExam(LessonId);

                var getExam = await _examService.GetExam(newExam);

                return Ok(getExam);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message,
                    exceptionType = ex.GetType().FullName,
                    stackTrace = ex.StackTrace
                });
            }
        }
        [Authorize]
        [HttpGet("get-exam")]
        public async Task<IActionResult> GetExam(int ExamId) //GetDetails
        {
            var Exam = await _examService.GetExam(ExamId);
            return Ok(Exam);
        }

        [Authorize]
        [HttpGet("open-exam")]
        public async Task<IActionResult> OpenExam(int ExamId) //Get Details + Questions
        {
            var Exam = await _examService.GetExamWithQuestions(ExamId);
            return Ok(Exam);
        }
        [Authorize]
        [HttpPost("submit-exam")]
        public async Task<IActionResult> SubmitExam(SubmitDto submitDto)
        {
            try
            {
                var sumbit = await _examService.SubmitExam(submitDto);
                return Ok(sumbit);
            }
            catch(Exception ex)
{
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [Authorize]
        [HttpGet("get-review")]
        public async Task<IActionResult> Review(int Id)
        {
            try
            {
                var response = await _reviewService.ReviewQuestions(
                    Id,
                    Domain.bublesheet.Entities.enums.submitType.Exam);

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
        [HttpPost("create-exam")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddExam(AddExamDto addExamDto)
        {
                await _examService.AddExamAsync(addExamDto);

                return Ok("Added Done");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-exam")]
        public async Task<IActionResult> DeleteExam(int Id)
        {
            await _examService.DeleteExam(Id);
            return Ok("Deleted Done");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("edit-exam")]
        public async Task<IActionResult> EditExam(EditExamDto examDto)
        {
            var EditedExam = await _examService.EditExam(examDto);
            return Ok(EditedExam);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Update-Exam-Questions")]
        public async Task<IActionResult> UpdateExamQuestions(
     [FromBody] UpdateExamQuestionsDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Invalid request.");

                if (dto.ExamId <= 0)
                    return BadRequest("Invalid ExamId.");

                if (dto.QuestionIds == null)
                    return BadRequest("QuestionIds is required.");

                await _examService.UpdateExamQuestions(dto);

                return Ok(new
                {
                    message = "Exam questions updated successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.InnerException?.Message
                });
            }
        }


    }
}
