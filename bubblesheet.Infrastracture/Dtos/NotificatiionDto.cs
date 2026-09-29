using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public NotificationType Type { get; set; }
        public int? ReferenceId { get; set; }
        public bool IsMark { get; set; }
    }
    public class MarkNotificationsAsReadDto
    {
        public List<int> NotificationIds { get; set; } = [];
    }
}
