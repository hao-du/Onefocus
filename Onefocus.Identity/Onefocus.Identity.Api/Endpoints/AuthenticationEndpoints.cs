using MediatR;
using Onefocus.Common.Results;
using Onefocus.Identity.Application.UseCases.Authentication.Commands;
using static Onefocus.Common.Results.ResultExtensions;

namespace Onefocus.Identity.Api.Endpoints;

internal static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("authenticate", async (AuthenticateCommandRequest request, ISender sender, HttpContext context) =>
        {
            var result = await sender.Send(request);

            return result.ToResult();
        });

        app.MapPost("logout", async (ISender sender) =>
        {
            var result = await sender.Send(new LogOutCommandRequest());

            return result.ToResult();
        });

        app.MapGet("refresh", async (ISender sender) =>
        {
            var result = await sender.Send(new RefreshTokenCommandRequest());
            if (result.IsFailure)
            {
                return result.ToNotAcceptableResult();
            }
            return result.ToResult();
        });
    }
}
