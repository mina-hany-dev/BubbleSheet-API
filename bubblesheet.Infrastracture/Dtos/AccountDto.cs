using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class RegesterDto
    {
        public string Name { get; set; }
        public string password { get; set; }
        public string ConfarimPassword { get; set; }
        public string Email { get; set; }
        public string phoneNumber { get; set; }
        public string School { get; set; }
        public bool Gender { get; set; }
        public string ParentphoneNumber { get; set; }
    }
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string role { get; set; }
    }
    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; }
    }
    public class CodeChargeDto
    {
        public string Text { get; set; }
    }
    public class BuySubjectDto
    {
        public int SubjectId { get; set; }
    }
    
}
