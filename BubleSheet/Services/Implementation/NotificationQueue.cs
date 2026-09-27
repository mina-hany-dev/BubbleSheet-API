using BubleSheet.Services.Interfaces;
using System.Threading.Channels;

namespace BubleSheet.Services.Implementation
{
    public class NotificationQueue : INotificationQueue
    {
        private readonly Channel<int> _queue;

        public NotificationQueue()
        {
            _queue = Channel.CreateUnbounded<int>();
        }

        public async ValueTask EnqueueAsync(int notificationId)
        {
            await _queue.Writer.WriteAsync(notificationId);
        }

        public async ValueTask<int> DequeueAsync(
            CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}