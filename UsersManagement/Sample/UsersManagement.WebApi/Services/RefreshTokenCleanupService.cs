using Microsoft.EntityFrameworkCore;
using UsersManagement.WebApi.DataContext;

namespace UsersManagement.WebApi.Services;

// Services/RefreshTokenCleanupService.cs
public class RefreshTokenCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RefreshTokenCleanupService> _logger;

    public RefreshTokenCleanupService(
        IServiceProvider serviceProvider,
        ILogger<RefreshTokenCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var expiredTokens = await dbContext.ApplicationRefreshTokens
                    //.Where(t => !(t.Expires.ToString("g").Contains("000")) && (t.IsExpired || t.IsRevoked))
                    .ToListAsync(stoppingToken);

                if (expiredTokens.Any())
                {
                    dbContext.ApplicationRefreshTokens.RemoveRange(expiredTokens);
                    await dbContext.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation($"Cleaned up {expiredTokens.Count} expired refresh tokens");
                }
            }

            // Run once per day
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
