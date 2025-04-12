using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Onefocus.Common.Results;
using Onefocus.Identity.Application.Authentication.Commands;
using static Onefocus.Common.Results.ResultExtensions;

namespace Onefocus.Identity.Api.Endpoints;

internal static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("authenticate", async (AuthenticateCommandRequest request, ISender sender, HttpContext context) =>
        {
            var result = await sender.Send<Result<AuthenticateCommandResponse>>(request);

            return result.ToResult();
        });

        app.MapGet("refresh", async (ISender sender) =>
        {
            var result = await sender.Send<Result<RefreshTokenCommandReponse>>(new RefreshTokenCommandRequest());
            if (result.IsFailure)
            {
                return result.ToNotAcceptableResult();
            }
            return result.ToResult();
        });
    }
}
