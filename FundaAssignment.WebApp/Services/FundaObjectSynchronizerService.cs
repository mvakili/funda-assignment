using FundaAssignment.WebApp.Services.Contracts;

namespace FundaAssignment.WebApp.Services;

/// <summary>
/// Keeps Objects updated with Funda API by refreshing data 
/// </summary>
public class FundaObjectSynchronizerService : IHostedService
{
    private readonly IObjectsService objectsService;
    private Timer timer = null!;
    public FundaObjectSynchronizerService(IObjectsService objectsService)
    {
        this.objectsService = objectsService;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Seeding Objects
        await ReloadObjectsAsync();

        timer = new Timer(StartReloadinObjects, null, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));

        await Task.CompletedTask;
    }

    private async Task ReloadObjectsAsync()
    {
        var tasks = new Task[] {
            objectsService.ReloadAmsterdamObjectsAsync(),
            objectsService.ReloadAmsterdamObjectsWithTuinAsync()
        };
        await Task.WhenAll(tasks);
    }

    private void StartReloadinObjects(object? state)
    {
        Task.Run(() => ReloadObjectsAsync());
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
}
