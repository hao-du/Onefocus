using MediatR;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.User.Commands;

namespace Onefocus.Membership.Api.Endpoints;

internal static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("user/all", async (ISender sender) =>
        {
            Result<GetAllUsersQueryResponse> result = await sender.Send(new GetAllUsersQueryRequest());

            return result.ToResult();
        }).RequireAuthorization();

        app.MapGet("user/{id}", async (Guid id, ISender sender) =>
        {
            Result<GetAllUsersQueryResponse> result = await sender.Send(new GetAllUsersQueryRequest());

            return result.ToResult();
        }).RequireAuthorization();

        app.MapPost("user/create", async (CreateUserCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);

            return result.ToResult();
        }).RequireAuthorization();

        app.MapPut("user/update", async (UpdateUserCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);

            return result.ToResult();
        }).RequireAuthorization();

        app.MapPatch("user/password/update", async (UpdatePasswordCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);

            return result.ToResult();
        }).RequireAuthorization();
    }
}
