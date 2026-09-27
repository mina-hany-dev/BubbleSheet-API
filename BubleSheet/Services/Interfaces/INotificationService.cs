using bubblesheet.Infrastracture.Dtos;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Microsoft.AspNetCore.Mvc;

public interface INotificationService
{
    Task CreateAsync(
        NotificationType type,
        int? referenceId);

    Task<List<NotificationDto>> GetStudentNotificationsAsync();

    Task<List<NotificationDto>> GetUnreadNotificationsAsync();

    Task<int> GetUnreadCountAsync();

    Task MarkAsReadAsync(MarkNotificationsAsReadDto dto);
}