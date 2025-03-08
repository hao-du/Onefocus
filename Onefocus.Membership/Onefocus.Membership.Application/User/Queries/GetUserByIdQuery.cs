using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using System.Linq;
using RepoRes = Onefocus.Membership.Infrastructure.Databases.Repositories.GetUserByIdRepositoryResponse;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record GetUserByIdQueryResponse(GetUserByIdQueryResponse.UserResponse User)
{
    public sealed record UserResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleResponse> Roles);
    public sealed record RoleResponse(Guid Id, string? RoleName);
    public static GetUserByIdQueryResponse CastFrom(RepoRes source)
    {
        var user = new UserResponse(
            source.User.Id
            , source.User.UserName
            , source.User.Email
            , source.User.FirstName
            , source.User.LastName
            , source.User.Roles.Select(r => new RoleResponse(r.Id, r.RoleName)).ToList()
        );

        return new(user);
    }
}

public sealed record GetUserByIdQueryRequest(Guid Id) : IQuery<GetUserByIdQueryResponse>
{
    public GetUserByIdRepositoryRequest ConvertTo() => new(Id);
}

internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQueryRequest, GetUserByIdQueryResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetUserByIdQueryResponse>> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetUserByIdAsync(request.ConvertTo());
        if (userResult.IsFailure)
        {
            return Result.Failure<GetUserByIdQueryResponse>(userResult.Error);
        }

        return Result.Success<GetUserByIdQueryResponse>(GetUserByIdQueryResponse.CastFrom(userResult.Value));
    }
}

