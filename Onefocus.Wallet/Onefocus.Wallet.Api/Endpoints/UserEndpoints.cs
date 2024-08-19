using MediatR;
using Microsoft.IdentityModel.Tokens;
using Onefocus.Common.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Onefocus.Wallet.Api.Endpoints;

internal static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();
    }
}
