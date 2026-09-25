using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class Student
    {
        public int StudentId { get; private set; }
        public string StudentName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string HashPassword { get; private set; }
        public bool Gender { get; private set; }
        public string School { get; private set; }
        public string ParentPhoneNumber { get; private set; }
        public decimal Balance { get; private set; }
        public DateTime? LastLogin { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }
        //public bool IsActive { get; private set; }

        private Student() { }

        public Student(string studentName, string phoneNumber, string email, string password, bool gender, string school, string parentPhoneNumber)
        {
            ValidateStudentName(studentName);
            ValidatePhoneNumber(phoneNumber);
            ValidateEmail(email);
            ValidatePassword(password);

            StudentName = studentName;
            PhoneNumber = phoneNumber.Trim();
            Email = email.Trim().ToLower();
            HashPassword = password;

            Balance = 0;
            Gender = gender;
            School = school;
            ParentPhoneNumber = parentPhoneNumber;
        }

        #region Validation Methods

        private void ValidateStudentName(string studentName)
        {
            if (string.IsNullOrWhiteSpace(studentName))
                throw new ArgumentException("Student name is required.");

            if (studentName.Length < 3 || studentName.Length > 100)
                throw new ArgumentException("Student name must be between 3 and 100 characters.");
        }

        private void ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number is required.");

            var phoneRegex = @"^01[0-2,5]{1}[0-9]{8}$";

            if (!Regex.IsMatch(phoneNumber, phoneRegex))
                throw new ArgumentException("Invalid Egyptian phone number.");
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                throw new ArgumentException("Invalid email format.");
            }
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.");

            if (password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters.");
        }

        #endregion

        #region Business Methods
        public void LastLoginUpdate()
        {
            this.LastLogin = DateTime.UtcNow;
        }
        public void SetRefreshToken(string refreshToken, DateTime expiry)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException("Refresh token is required.");

            if (expiry <= DateTime.UtcNow)
                throw new ArgumentException("Refresh token expiry must be in the future.");

            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = expiry;
        }

        public void AddBalance(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            Balance += amount;
        }

        public void DeductBalance(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            if (amount > Balance)
                throw new InvalidOperationException("Insufficient balance.");

            Balance -= amount;
        }

        public void ChangePassword(string newPassword)
        {
            ValidatePassword(newPassword);

            HashPassword = newPassword;
        }
        public void Logout()
        {
            RefreshToken = null;
            RefreshTokenExpiryTime = null;
        }
        #endregion
    }
}
