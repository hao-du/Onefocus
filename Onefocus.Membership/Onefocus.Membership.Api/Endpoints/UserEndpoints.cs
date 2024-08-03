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

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToBadRequestProblemDetails();
        });

        app.MapGet("user/{id}", async (Guid id, ISender sender) =>
        {
            Result<GetAllUsersQueryResponse> result = await sender.Send(new GetAllUsersQueryRequest());

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToBadRequestProblemDetails();
        });

        app.MapPost("user/create", async (CreateUserCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);

            return result.IsSuccess ? Results.Ok() : result.ToBadRequestProblemDetails();
        });

        app.MapPut("user/update", async (UpdateUserCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);

            return result.IsSuccess ? Results.Ok() : result.ToBadRequestProblemDetails();
        });

        app.MapPatch("user/password/update", async (UpdatePasswordCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);

            return result.IsSuccess ? Results.Ok() : result.ToBadRequestProblemDetails();
        });
    }
}
