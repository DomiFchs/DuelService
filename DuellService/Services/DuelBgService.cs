namespace DuellService.Services; 

public class DuelBgService : BackgroundService
{    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(10_000, stoppingToken);
        }
    }
}