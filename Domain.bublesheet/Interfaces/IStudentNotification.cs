using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IStudentNotification
    {
        Task AddAsync(StudentNotification studentNotification);

        Task AddRangeAsync(
            IEnumerable<StudentNotification> studentNotifications);

        Task<StudentNotification?> GetAsync(
            int studentId,
            int notificationId);

        Task MarkAsReadAsync(
            int studentId,
            int notificationId);

        Task<int> GetUnreadCountAsync(int studentId);
        Task<List<StudentNotification>> GetByStudentIdAsync(int studentId);
    }
}
