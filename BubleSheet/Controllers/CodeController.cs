using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Implementation;
using BubleSheet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BubleSheet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodeController(ICodeService codeService, IAccountService accountService) : ControllerBase
    {
        private readonly ICodeService _codeService = codeService;
        private readonly IAccountService _accountService = accountService;
        [Authorize]
        [HttpPost("redeem")]
        public async Task<IActionResult> RedeemCode([FromBody] CodeChargeDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Text))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Code is required."
                    });
                }

                await _codeService.ChargeCard(dto);

                return Ok(new
                {
                    success = true,
                    message = "Code redeemed successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while redeeming the code."
                });
            }
        }
        [Authorize]
        [HttpGet("gettransactiondata")]
        public async Task<IActionResult> GetTransactionData()
        {
            try
            {
                var transactions = await _accountService.GetTransactionDataAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add-codes")]
        public async Task<IActionResult> CreateCode([FromForm]AddCodeDto addCodeDto)
        {
            if (addCodeDto.balance <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "قيمة الرصيد غير صحيحة."
                });
            }

            if (addCodeDto.count <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "عدد الأكواد غير صحيح."
                });
            }
            var NewCodes = await _codeService.GenerateCodesAsync(addCodeDto.count, addCodeDto.balance);
            return Ok(NewCodes);
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("get-codes")]
        public async Task<IActionResult> GetCodes()
        {
            var Codes = await _codeService.GetAllCodesDtos();
            return Ok(Codes);
        }
    }
}
