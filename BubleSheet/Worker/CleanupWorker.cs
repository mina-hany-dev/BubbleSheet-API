using Domain.bublesheet.Interfaces;

namespace BubleSheet.Worker
{
    public class CleanupWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CleanupWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            using var timer =
                new PeriodicTimer(TimeSpan.FromDays(30));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope =
                    _scopeFactory.CreateScope();

                var repo =
                    scope.ServiceProvider
                        .GetRequiredService<INotification>();

                var unitOfWork =
                scope.ServiceProvider
                    .GetRequiredService<IUnitOfWork>();

                await repo.DeleteOldRowsAsync();
                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}
