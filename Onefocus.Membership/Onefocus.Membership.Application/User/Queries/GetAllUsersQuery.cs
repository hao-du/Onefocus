using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Membership.Infrastructure.Databases.Repositories;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record GetAllUsersQueryRequest() : IQuery<GetAllUsersQueryResponse>;

public sealed record GetAllUsersQueryResponse(List<GetAllUsersQueryResponse.UserResponse> Users)
{
    public static GetAllUsersQueryResponse CastFrom(GetAllUsersRepositoryResponse source)
    {
        var users = source.Users.Select(u => new UserResponse(
            u.Id, u.UserName, u.Email, u.FirstName, u.LastName,
            u.Roles.Select(r => new RoleRepsonse(r.Id, r.RoleName)).ToList()
        )).ToList();

        return new(users);
    }
    public sealed record UserResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleRepsonse> Roles);
    public sealed record RoleRepsonse(Guid Id, string? RoleName);
}


internal sealed class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetAllUsersQueryResponse>> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetAllUsersAsync();
        if (userResult.IsFailure)
        {
            return Result.Failure<GetAllUsersQueryResponse>(userResult.Errors);
        }

        return Result.Success<GetAllUsersQueryResponse>(GetAllUsersQueryResponse.CastFrom(userResult.Value));
    }
}

