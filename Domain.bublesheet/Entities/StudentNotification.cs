using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class StudentNotification
    {
        public int StudentId { get; private set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; private set; }

        public int NotificationId { get; private set; }
        [ForeignKey(nameof(NotificationId))]
        public Notification Notification { get; private set; }

        public bool IsRead { get; private set; }

        public DateTime? ReadAt { get; private set; }

        private StudentNotification()
        {
        }

        public StudentNotification(
            int studentId,
            int notificationId)
        {
            StudentId = studentId;
            NotificationId = notificationId;
            IsRead = false;
        }

        public void MarkAsRead()
        {
            if (IsRead)
                return;

            IsRead = true;
            ReadAt = DateTime.UtcNow;
        }
    }
}
