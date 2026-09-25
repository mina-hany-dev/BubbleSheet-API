using Domain.bublesheet.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniShop.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BubleSheet.Services.Implementation
{
    public class JwtServiceImplementation (IConfiguration config, IHttpContextAccessor httpContextAccessor) : IJwtService
    {
        private readonly IConfiguration _config = config;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string GenerateToken(Student student)
        {
            string role = "Student";
            if(student.Email == "ahmedbakrmiftah93@gmail.com")
            {
                role = "Admin";
            }
            else
            {
                role = "Student";
            }
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, student.StudentId.ToString()),
            new Claim(ClaimTypes.Email, student.Email),
            new Claim(ClaimTypes.Role,role)
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
     issuer: _config["Jwt:Issuer"],
     audience: _config["Jwt:Audience"],
     claims: claims,
     expires: DateTime.UtcNow.AddHours(1),
     signingCredentials: creds
 );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            string hash = BCrypt.Net.BCrypt.HashPassword(Convert.ToBase64String(randomBytes));

            return hash ;
        }
        public int GetCurrentStudentId()
        {
            var id = _httpContextAccessor.HttpContext?.User?
         .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(id, out var studentId))
                throw new UnauthorizedAccessException();

            return studentId;
        }
    }
}
