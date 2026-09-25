using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.bublesheet.Entities
{
    public class ResetPassword
    {
        public int Id { get; private set; }

        public string CodeOTP { get; private set; } = string.Empty;

        public DateTime CreatedAt { get; private set; }

        public DateTime ExpiredAt { get; private set; }
        public bool IsUsed { get; private set; }

        public int StudentId { get; private set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; private set; } = null!;

        private ResetPassword()
        {
            // EF Core
        }

        public ResetPassword(
            int studentId,
            string codeOTP,
            DateTime createdAt,
            DateTime expiredAt)
        {
            StudentId = studentId;
            CodeOTP = codeOTP;
            CreatedAt = createdAt;
            ExpiredAt = expiredAt;
            IsUsed = false;
        }

        public bool IsExpired(DateTime now)
        {
            return now >= ExpiredAt;
        }

        public bool Verify(string otp, DateTime now)
        {
            if (IsExpired(now))
                return false;

            return CodeOTP == otp;
        }
        public void UpdateIsUsed()
        {
            IsUsed = true;
        }
    }
}