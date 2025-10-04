using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Interfaces.Repositories.Read;
using Onefocus.Home.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Home.Domain.Entities.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Onefocus.Home.Application.UseCases.Setting.Queries;

public sealed record GetSettingByUserIdQueryRequest() : IQuery<GetSettingByUserIdQueryResponse>;
public sealed record GetSettingByUserIdQueryResponse(string Locale, string TimeZone);

internal sealed class GetSettingByUserIdQueryHandler(
    ILogger<GetSettingByUserIdQueryHandler> logger,
    IReadUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<GetSettingByUserIdQueryRequest, GetSettingByUserIdQueryResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<GetSettingByUserIdQueryResponse>> Handle(GetSettingByUserIdQueryRequest request, CancellationToken cancellationToken)
    {
        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult.Failure<GetSettingByUserIdQueryResponse>();

        var getSettingResult = await unitOfWork.Setting.GetSettingByUserIdAsync(new(actionByResult.Value), cancellationToken);
        if (getSettingResult.IsFailure) return getSettingResult.Failure<GetSettingByUserIdQueryResponse>();

        var preferences = getSettingResult.Value.Setting?.Preferences ?? Preferences.Default();
        return Result.Success<GetSettingByUserIdQueryResponse>(new(
            Locale: preferences.Locale,
            TimeZone: preferences.TimeZone
        ));
    }
}

