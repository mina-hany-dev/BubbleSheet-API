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
    public class NotificationRepo : INotification
    {
        private readonly bubblesheetDbContext _context;

        public NotificationRepo(bubblesheetDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);

            return notification;
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Notification>> GetUnreadByStudentIdAsync(int studentId)
        {
            return await _context.StudentNotifications
                .Where(x =>
                    x.StudentId == studentId &&
                    !x.IsRead)
                .Include(x => x.Notification)
                .Select(x => x.Notification)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        public async Task DeleteOldRowsAsync()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-30);

            var oldNotifications = await _context.Notifications
                .Where(x => x.CreatedAt < cutoffDate)
                .ToListAsync();

            _context.Notifications.RemoveRange(oldNotifications);
        }
    }
}
