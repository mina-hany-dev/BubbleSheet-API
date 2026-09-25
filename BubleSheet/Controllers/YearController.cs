using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubleSheet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class YearController(IYearService yearService) : ControllerBase
    {
        private readonly IYearService _yearService = yearService;
        [HttpPost("add-year")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddYear([FromForm] AddYearDto yearDto)
        {
            if (yearDto == null)
            {
                return BadRequest("Data Not Found");
            }
            else
            {
                var NewYear = await _yearService.AddAsync(yearDto);
                return Ok(NewYear);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("edit-year")]
        public async Task<IActionResult> EditYear(EditYearDto yearDto)
        {
            await _yearService.EditYear(yearDto);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-year")]
        public async Task <IActionResult> DeleteYear(int Id)
        {
            await _yearService.DeleteYear(Id);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-img-year")]
        public async Task<IActionResult> DeleteImgYear(int Id)
        {
            await _yearService.DeleteImgYear(Id);
            return Ok("Img Deleted");
        }
    }
}
