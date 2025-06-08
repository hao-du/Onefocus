using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.Interfaces.Repositories;

namespace Onefocus.Membership.Application.UseCases.User.Queries;

public sealed record GetAllUsersQueryRequest() : IQuery<GetAllUsersQueryResponse>;
public sealed record GetAllUsersQueryResponse(List<UserResponse> Users);
public sealed record UserResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName);

internal sealed class GetAllUsersQueryHandler(IUserRepository userRepository) : IQueryHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
{
    public async Task<Result<GetAllUsersQueryResponse>> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
    {
        var userResult = await userRepository.GetAllUsersAsync(cancellationToken);
        if (userResult.IsFailure)
        {
            return Result.Failure<GetAllUsersQueryResponse>(userResult.Errors);
        }

        var users = userResult.Value.Users;
        return Result.Success<GetAllUsersQueryResponse>(new(
            Users: [.. users.Select(u => new UserResponse(
                Id: u.Id,
                UserName: u.UserName,
                Email: u.Email,
                FirstName: u.FirstName,
                LastName: u.LastName
            ))]
        ));
    }
}

