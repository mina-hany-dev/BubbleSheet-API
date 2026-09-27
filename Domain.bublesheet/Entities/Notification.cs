using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class Notification
    {
        public int Id { get; private set; }

        public NotificationType Type { get; private set; }

        // ExamId, QuestionBankId, LessonId, AdId
        public int? ReferenceId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ICollection<StudentNotification> StudentNotifications { get; private set; }
            = new List<StudentNotification>();

        private Notification()
        {
        }

        public Notification(
            NotificationType type,
            int? referenceId)
        {
            Type = type;
            ReferenceId = referenceId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
