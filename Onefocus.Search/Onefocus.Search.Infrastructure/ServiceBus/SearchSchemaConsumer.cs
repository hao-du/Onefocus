using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Common.Results;
using Onefocus.Search.Application.Contracts;
using Onefocus.Search.Application.Interfaces.Services;
using Onefocus.Search.Infrastructure.Services;
using System.Text.Json;
using System.Threading;

namespace Onefocus.Search.Infrastructure.ServiceBus;

internal class SearchSchemaConsumer(
    ISearchSchemaService searchSchemaService,
    ILogger<ISearchSchemaMessage> logger
    ) : IConsumer<ISearchSchemaMessage>
{
    public async Task Consume(ConsumeContext<ISearchSchemaMessage> context)
    {
        var message = context.Message;

        logger.LogInformation("Received schema change event: {SchemaName}", message.SchemaName);

        try
        {
            var upsertSchemaResult = await searchSchemaService.UpsertIndexMappings(message.Schema, context.CancellationToken);

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
            logger.LogError(ex, "Error processing schema change: {SchemaName}", message.SchemaName);
        }
    }
}
