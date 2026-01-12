using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Onefocus.Wallet.Application.Interfaces.Services.Search;

namespace Onefocus.Wallet.Application.BackgroundServices;

internal class SearchSchemaInitializerHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SearchSchemaInitializerHostedService> _logger;

    public SearchSchemaInitializerHostedService(
        IServiceProvider serviceProvider,
        ILogger<SearchSchemaInitializerHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var schemaService = scope.ServiceProvider.GetRequiredService<ISearchSchemaManagementService>();
                await schemaService.InitializeSchemaAsync();

                _logger.LogInformation("Search schema initialized successfully");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Search schema initialization failed. Retrying in 30s...");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}