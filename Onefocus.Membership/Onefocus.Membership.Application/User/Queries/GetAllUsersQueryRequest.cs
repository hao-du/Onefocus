using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Infrastructure.Databases.Repositories.User;
using System.Linq;
using RepoRes = Onefocus.Membership.Infrastructure.Databases.Repositories.User.GetAllUsersRepositoryResponse;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record GetAllUsersQueryResponse(List<GetAllUsersQueryResponse.UserResponse> Users): IResponseObject<GetAllUsersQueryResponse, RepoRes>
{
    public sealed record UserResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleResponse> Roles);
    public sealed record RoleResponse(Guid Id, string? RoleName);
    public static GetAllUsersQueryResponse Create(RepoRes source)
    {
        var users = source.Users.Select(u => new UserResponse(
            u.Id
            , u.UserName
            , u.Email
            , u.FirstName
            , u.LastName
            , u.Roles.Select(r => new RoleResponse(r.Id, r.RoleName)).ToList()
        )).ToList();

        return new(users);
    }
}

public sealed record GetAllUsersQueryRequest() : IQuery<GetAllUsersQueryResponse>;
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
            return Result.Failure<GetAllUsersQueryResponse>(userResult.Error);
        }

        return Result.Success<GetAllUsersQueryResponse>(GetAllUsersQueryResponse.Create(userResult.Value));
    }
}

