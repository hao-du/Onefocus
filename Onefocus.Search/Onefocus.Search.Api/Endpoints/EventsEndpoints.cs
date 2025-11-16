using MediatR;
using Onefocus.Common.Results;
using Onefocus.Search.Application.UseCases.Commands;

namespace Onefocus.Search.Api.Endpoints;

internal static class EventsEndpoints
{
    public static void MapSearchEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapPost("search/index", async (IndexCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });
    }
}
