using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Search.Application.Interfaces.Services;

namespace Onefocus.Search.Infrastructure.ServiceBus;

internal class SearchSchemaConsumer(
    ISearchSchemaService searchSchemaService,
    ILogger<ISearchSchemaMessage> logger
    ) : IConsumer<ISearchSchemaMessage>
{
    public async Task Consume(ConsumeContext<ISearchSchemaMessage> context)
    {
        var message = context.Message;

        logger.LogInformation("Received schema change event: {indexName}", message.IndexName);

        try
        {
            var upsertSchemaResult = await searchSchemaService.UpsertIndexMappings(new(
                IndexName: message.IndexName,
                Mappings: message.Mappings
            ), context.CancellationToken);

            if (upsertSchemaResult.IsFailure)
            {
                foreach (var error in upsertSchemaResult.Errors)
                {
                    logger.LogError("Error processing with schema with Code: {Code}, Description: {Description}", error.Code, error.Description);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing schema change: {indexName}", message.IndexName);
        }
    }
}
