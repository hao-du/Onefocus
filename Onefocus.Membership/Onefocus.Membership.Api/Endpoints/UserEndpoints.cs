using MediatR;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.User.Commands;

namespace Web.Api.Endpoints;

internal static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("user/all", async (ISender sender) =>
        {
            Result<GetAllUsersQueryResponse> result = await sender.Send(new GetAllUsersQueryRequest());

            return result.IsSuccess ? result.ToOk() : result.ToBadRequestProblemDetails();
        });

        app.MapGet("user/{id}", async (Guid id, ISender sender) =>
        {
            Result<GetUserByIdQueryResponse> result = await sender.Send(new GetUserByIdQueryRequest(id));

            return result.IsSuccess ? result.ToOk() : result.ToBadRequestProblemDetails();
        });

        app.MapPost("user/create", async (CreateUserCommandRequest request, ISender sender) =>
        {
            Result result = await sender.Send(request);

            return result.IsSuccess ? result.ToOk() : result.ToBadRequestProblemDetails();
        });

        app.MapPut("user/update", async (UpdateUserCommandRequest request, ISender sender) =>
        {
            Result result = await sender.Send(request);

            return result.IsSuccess ? result.ToOk() : result.ToBadRequestProblemDetails();
        });

        app.MapPatch("user/password/update", async (UpdatePasswordCommandRequest request, ISender sender) =>
        {
            Result result = await sender.Send(request);

            return result.IsSuccess ? result.ToOk() : result.ToBadRequestProblemDetails();
        });
    }
}
