using bubblesheet.Infrastracture.Data;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Repos
{
    public class StudentNotificationRepo : IStudentNotification
    {
        private readonly bubblesheetDbContext _context;

        public StudentNotificationRepo(bubblesheetDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            StudentNotification studentNotification)
        {
            await _context.StudentNotifications
                .AddAsync(studentNotification);
        }

        public async Task AddRangeAsync(
            IEnumerable<StudentNotification> studentNotifications)
        {
            await _context.StudentNotifications
                .AddRangeAsync(studentNotifications);
        }

        public async Task<StudentNotification?> GetAsync(
            int studentId,
            int notificationId)
        {
            return await _context.StudentNotifications
                .FirstOrDefaultAsync(x =>
                    x.StudentId == studentId &&
                    x.NotificationId == notificationId);
        }
        public async Task<List<StudentNotification>> GetByStudentIdAsync(int studentId)
        {
            return await _context.StudentNotifications
                .Where(x => x.StudentId == studentId)
                .Include(x => x.Notification)
                .OrderByDescending(x => x.Notification.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(
            int studentId,
            int notificationId)
        {
            var studentNotification = await GetAsync(
                studentId,
                notificationId);

            if (studentNotification == null)
                return;

            studentNotification.MarkAsRead();
        }

        public async Task<int> GetUnreadCountAsync(int studentId)
        {
            return await _context.StudentNotifications
                .CountAsync(x =>
                    x.StudentId == studentId &&
                    !x.IsRead);
        }
    }
}
