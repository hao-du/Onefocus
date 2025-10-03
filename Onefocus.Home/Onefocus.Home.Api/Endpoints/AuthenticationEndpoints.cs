using MediatR;
using Onefocus.Common.Results;
using static Onefocus.Common.Results.ResultExtensions;

namespace Onefocus.Home.Api.Endpoints;

internal static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapMethods("check", ["HEAD"], (ISender sender) =>
        {
            return Result.Success().ToResult();
        });
    }
}
