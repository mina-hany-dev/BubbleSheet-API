using System.Security.Cryptography;
using System.Text;

namespace BubleSheet.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
