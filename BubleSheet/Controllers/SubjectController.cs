using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Implementation;
using BubleSheet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bubblesheet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly IBunnyStorageService _bannyStorageService;

        public SubjectController(ISubjectService subjectService, IBunnyStorageService bannyStorageService)
        {
            _subjectService = subjectService;
            _bannyStorageService = bannyStorageService;
        }
        [Authorize]
        [HttpPost("buy")]
        public async Task<IActionResult> BuySubject([FromBody] BuySubjectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _subjectService.BuySubject(dto);

                return Ok(new
                {
                    message = "Subject purchased successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
        [Authorize]
        [HttpGet("get-lessons")]
        public async Task<IActionResult> GetSubjectDetails(int subjectId)
        {
            try
            {
                if (subjectId <= 0)
                    return BadRequest(new
                    {
                        message = "Invalid subject id."
                    });

                var result = await _subjectService.GetSubjectDetails(subjectId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    stack = ex.StackTrace
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add-subject")]
        public async Task<IActionResult> AddSubject([FromForm]AddSubjectDto subjectDto)
        {
            var NewSubject = await _subjectService.AddSubjectAsync(subjectDto);
            return Ok(NewSubject);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("edit-subject")]
        public async Task<IActionResult> editSubject([FromForm]EditSubjectDTO DTO)
        {
            var EditedSubject = await _subjectService.EditSubjectAsync(DTO);
            return Ok(EditedSubject);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-subject")]
        public async Task<IActionResult> DeleteSubject(int Id)
        {
            await _subjectService.DeleteSubject(Id);
            return Ok();
        }


        // Test
        [HttpPost]
        public async Task<IActionResult> AddImgToBunnyTest(IFormFile img)
        {
            if (img == null || img.Length == 0)
                return BadRequest("Image is required.");

            try
            {
                var result = await _bannyStorageService.UploadImageAsync(
                    img,
                    "Test"
                );

                return Ok(new
                {
                    success = true,
                    url = result.Url
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }
        [HttpGet("test-secure-url")]
        public IActionResult TestSecureUrl(
    [FromQuery] string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return BadRequest("Relative path is required.");

            var secureUrl = _bannyStorageService.GenerateSecureUrl(
                relativePath,
                30
            );

            return Ok(new
            {
                url = secureUrl
            });
        }
    }
}