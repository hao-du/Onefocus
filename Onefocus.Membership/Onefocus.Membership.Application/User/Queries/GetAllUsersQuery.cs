using MediatR;
using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.User.Services;
using Onefocus.Membership.Infrastructure.Databases.Repositories.User;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record GetAllUsersQueryResponse(List<GetAllUsersQueryResponse.UserResponse> Users) 
    : IResponseObject<GetAllUsersQueryResponse, GetAllUsersServiceResponse>
{
    public static GetAllUsersQueryResponse Create(GetAllUsersServiceResponse source) 
        => new(source.Users.Select(user => UserResponse.Create(user)).ToList());

    public sealed record UserResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleRepsonse> Roles)
    : IResponseObject<UserResponse, GetAllUsersServiceResponse.UserResponse>
    {
        public static UserResponse Create(GetAllUsersServiceResponse.UserResponse source)
        {
            List<RoleRepsonse> roles = source.Roles.Select(r => RoleRepsonse.Create(r)).ToList();

            return new(source.Id, source.UserName, source.Email, source.FirstName, source.LastName, roles);
        }
    }

    public sealed record RoleRepsonse(Guid Id, string? RoleName) : IResponseObject<RoleRepsonse, GetAllUsersServiceResponse.RoleRepsonse>
    {
        public static RoleRepsonse Create(GetAllUsersServiceResponse.RoleRepsonse source) => new(source.Id, source.RoleName);
    }
}

public sealed record GetAllUsersQuery() : IQuery<GetAllUsersQueryResponse>;
internal sealed class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, GetAllUsersQueryResponse>
{
    private readonly IUserService _userService;

    public GetAllUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<GetAllUsersQueryResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var queryResult = await _userService.GetAllUsersAsync();
        if (queryResult.IsFailure)
        {
            return Result.Failure<GetAllUsersQueryResponse>(queryResult.Error);
        }

        var response = GetAllUsersQueryResponse.Create(queryResult.Value);
        return Result.Success<GetAllUsersQueryResponse>(response);
    }
}

