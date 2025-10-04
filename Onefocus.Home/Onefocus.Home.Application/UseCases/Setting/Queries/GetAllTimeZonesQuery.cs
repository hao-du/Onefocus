using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;

namespace Onefocus.Home.Application.UseCases.Settings.Queries;

public sealed record GetAllTimeZonesRequest() : IQuery<GetAllTimeZonesResponse>;
public sealed record GetAllTimeZonesResponse(List<TimeZoneResponse> TimeZones);
public sealed record TimeZoneResponse(string Id, string DisplayName);

internal sealed class GetAllTimeZonesQueryHandler(
    ILogger<GetAllTimeZonesQueryHandler> logger,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<GetAllTimeZonesRequest, GetAllTimeZonesResponse>(httpContextAccessor, logger)
{
    public override Task<Result<GetAllTimeZonesResponse>> Handle(GetAllTimeZonesRequest request, CancellationToken cancellationToken)
    {
        var zones = TimeZoneInfo.GetSystemTimeZones();

        return Task.Run(() => Result.Success<GetAllTimeZonesResponse>(new(
            [..zones.Select(c => new TimeZoneResponse(
                c.Id,
                c.DisplayName
            ))]
        )));
    }
}

