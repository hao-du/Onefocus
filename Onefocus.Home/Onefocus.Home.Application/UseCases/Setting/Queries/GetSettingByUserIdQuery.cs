using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Interfaces.Repositories.Read;
using Onefocus.Home.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Home.Domain.Entities.ValueObjects;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Onefocus.Home.Application.UseCases.Settings.Queries;

public sealed record GetSettingsByUserIdQueryRequest() : IQuery<GetSettingsByUserIdQueryResponse>;
public sealed record GetSettingsByUserIdQueryResponse(string Locale, string TimeZone, string Language);

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
        var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        var culture = cultures.FirstOrDefault(c => c.Name.Equals(preferences.Locale, StringComparison.OrdinalIgnoreCase));

        return Result.Success<GetSettingsByUserIdQueryResponse>(new(
            Locale: preferences.Locale,
            TimeZone: preferences.TimeZone,
            Language: culture?.TwoLetterISOLanguageName ?? "en"
        ));
    }
}

