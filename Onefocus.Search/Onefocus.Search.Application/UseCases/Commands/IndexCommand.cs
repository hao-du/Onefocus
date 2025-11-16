using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Search.Application.Contracts;
using Onefocus.Search.Application.Interfaces.Services;
using System.Text.Json;

namespace Onefocus.Search.Application.UseCases.Commands;

public sealed record IndexCommandRequest(List<EnvelopRequest> envelops) : ICommand;
public sealed record EnvelopRequest(string? EntityType, string? EntityId, JsonElement Payload) : ICommand;

internal sealed class IndexCommandHandler(
    ILogger<IndexCommandHandler> logger,
    IIndexingService indexingService,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<IndexCommandRequest>(httpContextAccessor, logger)
{
    public override async Task<Result> Handle(IndexCommandRequest request, CancellationToken cancellationToken)
    {
        var bulkIndexResult = await indexingService.AddIndex([..request.envelops.Select(request => new SearchIndexDto(
             EntityId: request.EntityId,
             IndexName: request.EntityType,
             Payload: request.Payload
            ))
        ], cancellationToken);

        if (bulkIndexResult.IsFailure) return bulkIndexResult;

        return Result.Success();
    }
}
