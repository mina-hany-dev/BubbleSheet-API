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
    public class LessonsController(IRandomExamTempleteService randomExamTempleteservice,ILessonService lessonService,IRandomExamTemplate randomExamTemplate) : ControllerBase
    {
        private readonly ILessonService _lessonService = lessonService;
        private readonly IRandomExamTemplate _randomExamTemplate = randomExamTemplate;
        private readonly IRandomExamTempleteService _randomExamTempleteService = randomExamTempleteservice;
        [Authorize]
        [HttpGet("Get-Lesson-Details")]
        public async Task<IActionResult> GetLessonDetails(int lessonId)
        {
            if (lessonId <= 0)
                return BadRequest("Invalid lesson id.");

            var lesson = await _lessonService.GetLessonDetails(lessonId);

            if (lesson == null)
                return NotFound(new
                {
                    Message = "Lesson not found."
                });

            return Ok(lesson);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add-lesson")]
        public async Task<IActionResult> AddLesson([FromForm]AddLessonDTO DTO)
        {
            var NewLesson = await _lessonService.AddLessonAsync(DTO);
            return Ok(NewLesson);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("edit-lesson")]
        public async Task<IActionResult> EditLesson([FromForm] EditLessonDto DTO)
        {
            var EditedLesson = await _lessonService.EditLesson(DTO);
            return Ok(EditedLesson);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-lesson")]
        public async Task<IActionResult> DeleteLesson(int Id)
        {
            await _lessonService.DeleteLesson(Id);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("get-random-values")]
        public async Task<IActionResult> GetRandomValues(int Id)
        {
            var rand = await _randomExamTemplate.GetByLessonIdAsync(Id);
            return Ok(rand);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("edit-random-values")]
        public async Task<IActionResult> EditRandomVAluesExam(UpdateRandomExamTemplate template)
        {
            var rand = await _randomExamTempleteService.updateAsync(template);
            return Ok(rand);
        }
    }
}