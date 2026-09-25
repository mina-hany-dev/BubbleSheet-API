using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubleSheet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController(IQuestionService questionService,IDashboardService dashboardService): ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;
        private readonly IDashboardService _dashboardService = dashboardService;
        // Student Actions
        [Authorize]
        [HttpGet("get-qCount")]
        public async Task<IActionResult> CountOfQuestions()
        {
            var count = await _dashboardService.GetStudentSolvedData();
            return Ok(count.Item2);
        }


        //Teacher Actions
        [Authorize(Roles = "Admin")]
        [HttpPost("add-Question")]
        public async Task<IActionResult> addQuestions(
    [FromForm] AddQuestionsDto addQuestionsDto)
        {
            var question = await _questionService.AddQuestion(addQuestionsDto);

            return Ok(question);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-Question")]
        public async Task<IActionResult> DeleteQuestions(int Id)
        {
            await _questionService.DeleteQuestion(Id);
            return Ok("Question Deleted");
        }
    }
}
