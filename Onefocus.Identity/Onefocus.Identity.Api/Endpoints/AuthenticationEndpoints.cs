using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Onefocus.Common.Results;
using Onefocus.Identity.Application.Authentication.Commands;

namespace Onefocus.Identity.Api.Endpoints;

internal static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("authenticate", async (AuthenticateCommandRequest request, ISender sender) =>
        {
            var result = await sender.Send<Result<AccessTokenResponse>>(request);
            return result.ToResult();
        });

        app.MapPost("refresh", async (RefreshTokenCommandRequest request, ISender sender) =>
        {
            var result = await sender.Send<Result<AccessTokenResponse>>(request);
            return result.ToResult();
        });
    }
}
