using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;

public class NotificationWorker : BackgroundService
{
    private readonly INotificationQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;

    public NotificationWorker(
        INotificationQueue queue,
        IServiceScopeFactory scopeFactory)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var notificationId =
                await _queue.DequeueAsync(stoppingToken);

            using var scope =
                _scopeFactory.CreateScope();

            var studentRepo =
                scope.ServiceProvider
                    .GetRequiredService<IAccount>();

            var studentNotificationRepo =
                scope.ServiceProvider
                    .GetRequiredService<IStudentNotification>();

            var unitOfWork =
                scope.ServiceProvider
                    .GetRequiredService<IUnitOfWork>();

            var students =
                await studentRepo.GetAllStudents();

            var studentNotifications = students
                .Select(student =>
                    new StudentNotification(
                        student.StudentId,
                        notificationId))
                .ToList();

            await studentNotificationRepo
                .AddRangeAsync(studentNotifications);

            await unitOfWork.SaveChangesAsync();
        }
    }
}