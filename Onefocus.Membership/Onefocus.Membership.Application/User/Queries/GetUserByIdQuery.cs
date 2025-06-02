using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using RepoRes = Onefocus.Membership.Infrastructure.Databases.Repositories.GetUserByIdRepositoryResponse;

namespace Onefocus.Membership.Application.User.Queries;

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
            , [.. source.User.Roles.Select(r => new RoleResponse(r.Id, r.RoleName))]
        );

        return new(user);
    }
}

public sealed record GetUserByIdQueryRequest(Guid Id) : IQuery<GetUserByIdQueryResponse>
{
    public GetUserByIdRepositoryRequest ToObject() => new(Id);
}

internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserByIdQueryRequest, GetUserByIdQueryResponse>
{
    public async Task<Result<GetUserByIdQueryResponse>> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var userResult = await userRepository.GetUserByIdAsync(request.ToObject());
        if (userResult.IsFailure)
        {
            return Result.Failure<GetUserByIdQueryResponse>(userResult.Errors);
        }

        return Result.Success(GetUserByIdQueryResponse.CastFrom(userResult.Value));
    }
}

