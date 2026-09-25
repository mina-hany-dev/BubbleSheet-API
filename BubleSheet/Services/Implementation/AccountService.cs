using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MiniShop.Application.Interfaces;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BubleSheet.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IJwtService _jwtService;
        private readonly IAccount _account;
        private readonly ICode _code;
        private readonly ITransaction _transcation;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubject _subject;
        private readonly IstudentSubject _studentSubject;
        public AccountService(IAccount account,IJwtService jwtService, ICode code , ITransaction transaction,IUnitOfWork unitOfWork, ISubject subject,IstudentSubject studentSubject)
        {
            _account = account;
            _jwtService = jwtService;
            _code = code;
            _transcation = transaction; 
            _unitOfWork = unitOfWork;
            _subject = subject;
            _studentSubject = studentSubject;
        }

        public bool checkIfHeIsAdmin(string Email) // NOT SCALLED
        {
            if (Email == "ahmedbakrmiftah93@gmail.com")
                return true;
            return false;
        }

        public async Task Regester(RegesterDto regester)
        {
            if (regester == null)
                throw new ArgumentException("Registration data is required.");

            if (string.IsNullOrWhiteSpace(regester.Name))
                throw new ArgumentException("Name is required.");

            if (string.IsNullOrWhiteSpace(regester.Email))
                throw new ArgumentException("Email is required.");

            if (regester.password != regester.ConfarimPassword)
                throw new ArgumentException("Password and confirm password do not match.");

            var existStudent = await _account.GetStudentByEmailAsync(regester.Email);

            if (existStudent != null)
                throw new ArgumentException("Email already exists.");

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(regester.password);

            Student student = new Student(
                regester.Name,
                regester.phoneNumber,
                regester.Email,
                hashPassword,
                regester.Gender,
                regester.School,
                regester.ParentphoneNumber
            );

            await _account.AddAsync(student);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {
            if (loginDto == null)
                throw new ArgumentNullException(nameof(loginDto));

            var student = await _account.GetStudentByEmailAsync(loginDto.Email);

            if (student == null)
                throw new UnauthorizedAccessException("Invalid Email or Password");

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(
                loginDto.Password,
                student.HashPassword);

            if (!isValidPassword)
                throw new UnauthorizedAccessException("Invalid Email or Password");

            var accessToken = _jwtService.GenerateToken(student);
            var refreshToken = _jwtService.GenerateRefreshToken();
            student.SetRefreshToken(
                refreshToken,
                DateTime.UtcNow.AddDays(7));
            student.LastLoginUpdate();

            await _account.UpdateAsync(student);
            await _unitOfWork.SaveChangesAsync();

            return new LoginResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                role = checkIfHeIsAdmin(loginDto.Email) ? "Admin" : "Student"
            };
        }
        public async Task<LoginResponseDto> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new UnauthorizedAccessException("Invalid refresh token");

            var student =
                await _account.GetStudentByRefreshTokenAsync(refreshToken);

            if (student == null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            if (student.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expired");

            var newAccessToken =
                _jwtService.GenerateToken(student);

            var newRefreshToken =
                _jwtService.GenerateRefreshToken();

            student.SetRefreshToken(
                newRefreshToken,
                DateTime.UtcNow.AddDays(7));

            await _account.UpdateAsync(student);
            await _unitOfWork.SaveChangesAsync();

            return new LoginResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
            };
        }
        public async Task RechargeStudent(int studentId, decimal amount, TransactionType type, string reference)
        {
            var student = await _account.GetStudentByIDAsync(studentId);

            if (student == null)
                throw new Exception("Student not found.");

            decimal balanceBefore = student.Balance;

            student.AddBalance(amount);

            var transaction = new WalletTransactions
            {
                StudentId = student.StudentId,
                Amount = amount,
                BalanceBefore = balanceBefore,
                BalanceAfter = student.Balance,
                TransactionType = type,
                Reference = reference
            };

            await _transcation.AddAsync(transaction);
            await _account.UpdateAsync(student);
        }
        public async Task<List<WalletTransactionDto>> GetTransactionDataAsync()
        {
            int studentId = _jwtService.GetCurrentStudentId();

            var transactions = await _transcation.GetByStudentIdAsync(studentId);

            var totalCash = await _transcation.GetTotalCash(studentId);
            var totalPaid = await _transcation.GetTotalPaid(studentId);

            return transactions.Select(x => new WalletTransactionDto
            {
                Amount = x.Amount,
                BalanceBefore = x.BalanceBefore,
                BalanceAfter = x.BalanceAfter,
                TransactionType = x.TransactionType,
                Reference = x.Reference,
                CreatedAt = x.CreatedAt,
                TotalCash = totalCash,
                TotalPaid = totalPaid
            }).ToList();
        }
        public async Task DeductBalance(
    int studentId,
    decimal amount,
    TransactionType transactionType,
    string reference)
        {
            var student = await _account.GetStudentByIDAsync(studentId);

            if (student == null)
                throw new Exception("Student not found.");

            if (student.Balance < amount)
                throw new Exception("Insufficient balance.");

            decimal balanceBefore = student.Balance;

            student.DeductBalance(amount);

            var transaction = new WalletTransactions
            {
                StudentId = student.StudentId,
                Amount = amount,
                BalanceBefore = balanceBefore,
                BalanceAfter = student.Balance,
                TransactionType = transactionType,
                Reference = reference
            };

            await _transcation.AddAsync(transaction);
            await _account.UpdateAsync(student);
        }
        public async Task Logout()
        {
            int studentId = _jwtService.GetCurrentStudentId();

            var student = await _account.GetStudentByIDAsync(studentId);

            if (student == null)
                throw new UnauthorizedAccessException("Student not found");

            student.Logout();

            await _account.UpdateAsync(student);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
