namespace BubleSheet.Services.Interfaces
{
    public interface INotificationQueue
    {
        ValueTask EnqueueAsync(int notificationId);

        ValueTask<int> DequeueAsync(
            CancellationToken cancellationToken);
    }
}