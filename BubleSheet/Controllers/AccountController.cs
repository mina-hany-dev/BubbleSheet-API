using bubblesheet.Infrastracture.Data;
using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BubleSheet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly IResetPasswordService _resetPasswordService;
        public AccountController(IAccountService account,IResetPasswordService resetPasswordService )
        {
            _account = account;
            _resetPasswordService = resetPasswordService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegesterDto register)
        {
            try
            {
                await _account.Regester(register);

                return Ok(new
                {
                    success = true,
                    message = "Registration successful."
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
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong while creating the account."
                });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(
            LoginDto loginDto)
        {
            try
            {
                var result =
            await _account.Login(loginDto);

                Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                });


                return Ok(
                    new ResponseLoginDto
                    {
                        role = result.role,
                        Token = result.Token
                    }
                    );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var result = await _account.RefreshToken(refreshToken);

            Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(result.Token);
        }
        [HttpPost("create-reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateNewResetPassword(string Email)
        {
            await _resetPasswordService.CreateNewOTP(Email);
            return Ok();
        }
        [HttpPost("verify-reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOTP(string Email,string OTP)
        {
            bool IsCorrect = await _resetPasswordService.VerifyOTP(Email,OTP);
            if (IsCorrect)
                return Ok();
            else
                return BadRequest();
        }
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult>ResetPassword(string Email,string NewPassword)
        {
            await _resetPasswordService.UpdatePassword(Email,NewPassword);
            return Ok();
        }
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _account.Logout();
            return(Ok());
        }
    }
}
