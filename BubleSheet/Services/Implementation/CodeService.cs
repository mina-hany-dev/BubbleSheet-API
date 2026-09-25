using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using MiniShop.Application.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class CodeService : ICodeService
    {
        private readonly ICode _code;
        private static readonly Random _random = new();
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly IJwtService _jwtService;
        public CodeService(ICode code , IUnitOfWork unitOfWork, IAccountService accountService, IJwtService jwtService)
        {
            _code = code;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _jwtService = jwtService;
        }
        public async Task<List<CodeDto>> GenerateCodesAsync(
            int count,
            decimal balance)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero");

            if (balance <= 0)
                throw new ArgumentException("Balance must be greater than zero");

            var codes = new List<Code>();

            for (int i = 0; i < count; i++)
            {
                string codeText;

                do
                {
                    codeText = GenerateRandomCode();
                }
                while (await _code.GetByTextAsync(codeText) != null);

                var code = new Code(
                    codeText,
                    balance);

                codes.Add(code);
            }

            await _code.AddRangeCodeAsync(codes);
            await _unitOfWork.SaveChangesAsync();
            return codes.Select(c => new CodeDto { Code = c.CodeText, IsUsed = c.Sold,Id = c.CodeId,CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(
    DateTime.SpecifyKind(c.CreatedAt, DateTimeKind.Utc),
    TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")), Amount = c.Amount}).ToList();
        }

        public async Task DeleteCode(string codeText)
        {
            if (string.IsNullOrWhiteSpace(codeText))
                throw new ArgumentException("Code text is required");

            var code = await _code.GetByTextAsync(codeText);

            if (code == null)
                throw new KeyNotFoundException("Code not found");

            await _code.DeleteAsync(code.CodeId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChargeCard(CodeChargeDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                int studentId = _jwtService.GetCurrentStudentId();

                var code = await _code.GetByTextAsync(dto.Text);

                if (code == null)
                    throw new ArgumentException("Invalid code.");

                if (code.Sold)
                    throw new InvalidOperationException("Code already used.");

                await _accountService.RechargeStudent(
                    studentId,
                    code.Amount,
                    TransactionType.RechargeCard,
                    code.CodeText);

                code.MarkAsSold();

                await _code.UpdateAsync(code);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task<List<CodeDto>> GetAllCodesDtos()
        {
            var codes = await _code.GetAllAsync();

            return codes.Select(c => new CodeDto
            {
                Code = c.CodeText,
                CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(
    DateTime.SpecifyKind(c.CreatedAt, DateTimeKind.Utc),
    TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")),
                Id = c.CodeId,
                IsUsed = c.Sold,
                Amount = c.Amount
            }).OrderByDescending(c=>c.CreatedAt).ToList();
        }
        private string GenerateRandomCode()
        {
            const string chars =
                "ABCDEFGHJKMNOPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz123456789";

            return new string(
                Enumerable.Range(0, 10)
                .Select(x => chars[_random.Next(chars.Length)])
                .ToArray());
        }
    }
}
