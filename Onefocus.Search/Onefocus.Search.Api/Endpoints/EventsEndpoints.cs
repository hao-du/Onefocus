using MediatR;
using Onefocus.Common.Results;

namespace Onefocus.Search.Api.Endpoints;

internal static class EventsEndpoints
{
    public static void MapSearchEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();
    }
}
