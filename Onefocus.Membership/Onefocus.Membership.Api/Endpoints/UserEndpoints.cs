using MediatR;
using Microsoft.IdentityModel.Tokens;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.User.Commands;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Onefocus.Membership.Api.Endpoints;

internal static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapGet("user/all", async (ISender sender) =>
        {
            Result<GetAllUsersQueryResponse> result = await sender.Send(new GetAllUsersQueryRequest());

            return result.ToResult();
        });

        routes.MapGet("user/{id}", async (Guid id, ISender sender) =>
        {
            Result<GetAllUsersQueryResponse> result = await sender.Send(new GetAllUsersQueryRequest());

            return result.ToResult();
        });

        routes.MapPost("user/create", async (CreateUserCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);

            return result.ToResult();
        });

        routes.MapPut("user/update", async (UpdateUserCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);

            return result.ToResult();
        });

        routes.MapPost("user/sync", async (SyncUserCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);

            return result.ToResult();
        });

        routes.MapPatch("user/password/update", async (UpdatePasswordCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);

            return result.ToResult();
        });
    }
}
