using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Interfaces.Repositories.Read;
using Onefocus.Home.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Home.Domain.Entities.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Onefocus.Home.Application.UseCases.Settings.Queries;

public sealed record GetSettingsByUserIdQueryRequest() : IQuery<GetSettingsByUserIdQueryResponse>;
public sealed record GetSettingsByUserIdQueryResponse(string Locale, string TimeZone);

internal sealed class GetSettingsByUserIdQueryHandler(
    ILogger<GetSettingsByUserIdQueryHandler> logger,
    IReadUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<GetSettingsByUserIdQueryRequest, GetSettingsByUserIdQueryResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<GetSettingsByUserIdQueryResponse>> Handle(GetSettingsByUserIdQueryRequest request, CancellationToken cancellationToken)
    {
        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult.Failure<GetSettingsByUserIdQueryResponse>();

        var getSettingResult = await unitOfWork.Settings.GetSettingsByUserIdAsync(new(actionByResult.Value), cancellationToken);
        if (getSettingResult.IsFailure) return getSettingResult.Failure<GetSettingsByUserIdQueryResponse>();

        var preferences = getSettingResult.Value.Settings?.Preferences ?? Preferences.Default();
        return Result.Success<GetSettingsByUserIdQueryResponse>(new(
            Locale: preferences.Locale,
            TimeZone: preferences.TimeZone
        ));
    }
}

