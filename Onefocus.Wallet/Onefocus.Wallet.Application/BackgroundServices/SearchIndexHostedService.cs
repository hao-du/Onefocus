using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Onefocus.Wallet.Application.Interfaces.Services.Search;

namespace Onefocus.Wallet.Application.BackgroundServices;

internal class SearchIndexHostedService(
    IServiceProvider serviceProvider,
    ILogger<SearchIndexHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("SearchIndexHostedService started");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var indexService = scope.ServiceProvider.GetRequiredService<ISearchIndexManagementService>();
                var result = await indexService.ExecuteSearchIndexAsync(cancellationToken);

                logger.LogInformation("Search schema initialized successfully");
                break;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Search schema initialization failed. Retrying in 30s...");
                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }
    }
}