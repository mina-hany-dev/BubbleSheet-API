using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubleSheet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfController(IpdfService ipdfService) : ControllerBase
    {
        private readonly IpdfService _pdfService = ipdfService;
        [Authorize(Roles = "Admin")]
        [HttpPost("add-pdf")]
        public async Task<IActionResult> AddPdfAsync([FromForm]AddPdfDto Dto)
        {
            var pdf = await _pdfService.AddPdfAsync(Dto);
            return Ok(pdf);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("remove-pdf")]
        public async Task<IActionResult> removePdfAsync(int PdfId)
        {
            await _pdfService.DeletePdf(PdfId);
            return Ok();
        }
        [Authorize]
        [HttpGet("download-pdf")]
        public async Task<IActionResult> DownloadPdfAsync(int pdfId)
        {
            try
            {
                var url = await _pdfService.GetPdfUrlAsync(pdfId);

                return Ok(new
                {
                    url
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = "PDF not found."
                });
            }
        }
    }
}
