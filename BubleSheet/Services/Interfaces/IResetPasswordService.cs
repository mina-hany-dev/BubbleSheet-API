namespace BubleSheet.Services.Interfaces
{
    public interface IResetPasswordService
    {
        Task CreateNewOTP(string StudentEmail);
        Task<bool> VerifyOTP(string studentEmail, string otp);
        Task UpdatePassword(
    string studentEmail,
    string newPassword);
    }
}
