using bubblesheet.Infrastracture.Dtos;
using Domain.bublesheet.Entities.enums;

namespace BubleSheet.Services.Interfaces
{
    public interface IAccountService
    {
        Task Regester(RegesterDto regester);
        Task<LoginResponseDto> Login(LoginDto loginDto);
        Task<LoginResponseDto> RefreshToken(string refreshToken);
        Task RechargeStudent(int studentId, decimal amount, TransactionType type, string reference);
        Task<List<WalletTransactionDto>> GetTransactionDataAsync();
        Task DeductBalance(int studentId, decimal amount, TransactionType transactionType, string reference);
        bool checkIfHeIsAdmin(string Email);
        Task Logout();
    }
}
