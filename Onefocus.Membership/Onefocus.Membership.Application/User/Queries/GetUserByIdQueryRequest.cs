using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Infrastructure.Databases.Repositories.User;
using System.Linq;
using RepoRes = Onefocus.Membership.Infrastructure.Databases.Repositories.User.GetUserByIdRepositoryResponse;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record GetUserByIdQueryResponse(GetUserByIdQueryResponse.UserResponse User): IResponseObject<GetUserByIdQueryResponse, RepoRes>
{
    public sealed record UserResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleResponse> Roles);
    public sealed record RoleResponse(Guid Id, string? RoleName);
    public static GetUserByIdQueryResponse Create(RepoRes source)
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

public sealed record GetUserByIdQueryRequest(Guid Id) : IQuery<GetUserByIdQueryResponse>, IRequestObject<GetUserByIdRepositoryRequest>
{
    public GetUserByIdRepositoryRequest ToRequestObject() => new(Id);
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
        var userResult = await _userRepository.GetUserByIdAsync(request.ToRequestObject());
        if (userResult.IsFailure)
        {
            return Result.Failure<GetUserByIdQueryResponse>(userResult.Error);
        }

        return Result.Success<GetUserByIdQueryResponse>(GetUserByIdQueryResponse.Create(userResult.Value));
    }
}

