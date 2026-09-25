using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubleSheet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdController(IAdvertiserService advertiser,IImgadService imgadService) : ControllerBase
    {
        private readonly IAdvertiserService _advertiser = advertiser;
        private readonly IImgadService _imgadService = imgadService;
        [HttpGet("get-ads")]
        public async Task<IActionResult> GetAllAds()
        {
            var ads = await _advertiser.GetAllAdvertiser();
            return Ok(ads);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add-ad")]
        public async Task<IActionResult> AddDavertiser([FromForm]AddAdvertiserDto advertisersDto)
        {
            await _advertiser.AddAsyncAdvertiser(advertisersDto);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-ad")]
        public async Task<IActionResult> DeleteDavertiser(int adId)
        {
            await _advertiser.DeleteAdvertiser(adId);
            return Ok();
        }
        // IMGADS
        [Authorize(Roles="Admin")]
        [HttpPost("add-imgad")]
        public async Task<IActionResult> AddImgAds([FromForm]AddImgAdDTO adDTO)
        {
            var newAdimg = await _imgadService.AddAdAsync(adDTO);
            return Ok(newAdimg);
        }
        [Authorize(Roles ="Admin")]
        [HttpDelete("Delete-imgad")]
        public async Task<IActionResult> DeleteImgAd(int id)
        {
            try
            {
                await _imgadService.DeleteAdAsync(id);

                return Ok(new
                {
                    success = true,
                    message = "Advertisement deleted successfully."
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Advertisement not found."
                });
            }
        }
        [HttpGet("get-imgad")]
        [Authorize]
        public async Task<IActionResult> GetimgAd(Levels level)
        {
            var adimg = await _imgadService.GetImgAdByLevel(level);
            return Ok(adimg);
        }
    }
}
