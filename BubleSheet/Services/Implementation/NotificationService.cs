using bubblesheet.Infrastracture.Dtos;
using bubblesheet.Infrastracture.Repos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using MiniShop.Application.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly INotification _notificationRepo;
        private readonly IStudentNotification _studentNotificationRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationQueue _notificationQueue;
        private readonly IStudentNotification _studentNotification;
        private readonly IJwtService _jwtService;

        public NotificationService(
            INotification notificationRepo,
            IStudentNotification studentNotificationRepo,
            IUnitOfWork unitOfWork,
            INotificationQueue notificationQueue,
            IJwtService jwtService,
            IStudentNotification studentNotification)
        {
            _notificationRepo = notificationRepo;
            _studentNotificationRepo = studentNotificationRepo;
            _unitOfWork = unitOfWork;
            _notificationQueue = notificationQueue;
            _jwtService = jwtService;
            _studentNotification = studentNotification;
        }

        public async Task CreateAsync(
    NotificationType type,
    int? referenceId)
        {
            var notification = new Notification(
                type,
                referenceId);

            await _notificationRepo.AddAsync(notification);

            await _unitOfWork.SaveChangesAsync();

            await _notificationQueue.EnqueueAsync(
                notification.Id);
        }
        public async Task<List<NotificationDto>> GetStudentNotificationsAsync()
        {
            int StudentId = _jwtService.GetCurrentStudentId();
            var notifications = await _studentNotification
                .GetByStudentIdAsync(StudentId);

            return notifications
                .Select(x => new NotificationDto
                {
                    Id = x.NotificationId,
                    Type = x.Notification.Type,
                    ReferenceId = x.Notification.ReferenceId,
                    IsMark = x.IsRead
                })
                .ToList();
        }

        public async Task<List<NotificationDto>> GetUnreadNotificationsAsync()
        {
            int StudentId = _jwtService.GetCurrentStudentId();
            var unReadNotification = await _notificationRepo
                .GetUnreadByStudentIdAsync(StudentId);
            return unReadNotification
               .Select(x => new NotificationDto
               {
                   Id = x.Id,
                   Type = x.Type,
                   ReferenceId = x.ReferenceId,
                   IsMark = false
               })
               .ToList();
        }

        public async Task<int> GetUnreadCountAsync()
        {
            int StudentId = _jwtService.GetCurrentStudentId();
            return await _studentNotificationRepo
                .GetUnreadCountAsync(StudentId);
        }

        public async Task MarkAsReadAsync(MarkNotificationsAsReadDto dto)
        {
            int StudentId = _jwtService.GetCurrentStudentId();
            foreach (var notification in dto.NotificationIds)
            {
                var studentNotification =
                await _studentNotificationRepo.GetAsync(
                    StudentId,
                    notification);

                if (studentNotification == null)
                    continue;

                studentNotification.MarkAsRead();
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
