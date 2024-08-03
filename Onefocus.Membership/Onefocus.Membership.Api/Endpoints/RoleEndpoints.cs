using MediatR;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.User.Commands;

namespace Onefocus.Membership.Api.Endpoints;

internal static class RoleEndpoints
{
    public static void MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("role/create", async (UpdateUserCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);

            return result.ToResult();
        });
    }
}
