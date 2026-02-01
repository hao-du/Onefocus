using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Onefocus.Search.Application.Interfaces.Services;

namespace Onefocus.Search.Application.BackgroundServices;

internal class SearchIndexHostedService(
    IServiceProvider serviceProvider,
    ILogger<SearchIndexHostedService> logger) : BackgroundService
{
    private static readonly TimeSpan DelayForEachExecution = TimeSpan.FromSeconds(30);

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

                if (result.IsFailure)
                {
                    foreach (var error in result.Errors)
                    {
                        logger.LogError("Error when adding index with Code: {Code}, Description: {Description}", error.Code, error.Description);
                    }
                }

                logger.LogInformation("Search schema initialized successfully");

                await Task.Delay(DelayForEachExecution, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Search index failed. Retrying in 30s...");
                await Task.Delay(DelayForEachExecution, cancellationToken);
            }
        }
    }
}