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

internal class SearchIndexConsumer(
    IIndexingService indexingService,
    ILogger<SearchIndexConsumer> logger
    ) : IConsumer<ISearchIndexMessage>
{
    private static readonly JsonSerializerOptions _payloadJsonOptions = new()
    {
        PropertyNameCaseInsensitive = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public async Task Consume(ConsumeContext<ISearchIndexMessage> context)
    {
        var searchIndexDtos = context.Message.Entities.Select(entity => new SearchIndexDto(
            EntityId: entity.EntityId,
            IndexName: entity.IndexName,
            Payload: JsonSerializer.Deserialize<JsonElement>(entity.Payload, _payloadJsonOptions)
        )).ToList();

        var indexResult = await indexingService.AddIndex(searchIndexDtos, context.CancellationToken);

        if (indexResult.IsFailure)
            LogError(indexResult);
    }

    private void LogError(Result result)
    {
        foreach (var error in result.Errors)
        {
            logger.LogError("Error when bulk-indexing with Code: {Code}, Description: {Description}", error.Code, error.Description);
        }
        throw new InvalidOperationException($"Error when bulk-indexing with Code: {result.Error.Code}, Description: {result.Error.Description}");
    }
}
