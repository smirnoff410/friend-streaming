namespace Translation.BackgroundServices;

public class TranslationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public TranslationBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var udpService = scope.ServiceProvider.GetRequiredService<IUdpListenerService>();
            await udpService.StartListener();
        }
    }
}