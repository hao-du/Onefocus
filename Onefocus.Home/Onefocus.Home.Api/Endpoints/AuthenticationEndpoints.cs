using MediatR;
using Onefocus.Common.Results;
using static Onefocus.Common.Results.ResultExtensions;

namespace Onefocus.Home.Api.Endpoints;

internal static class HomeEndpoints
{
    public static void MapHomeEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapMethods("check", ["HEAD"], (ISender sender) =>
        {
            return Result.Success().ToResult();
        });
    }
}
