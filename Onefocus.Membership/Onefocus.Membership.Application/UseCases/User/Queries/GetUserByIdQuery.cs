using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.Interfaces.Repositories;

namespace Onefocus.Membership.Application.UseCases.User.Queries;

public sealed record GetUserByIdQueryRequest(Guid Id) : IQuery<GetUserByIdQueryResponse>;
public sealed record GetUserByIdQueryResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName);

internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserByIdQueryRequest, GetUserByIdQueryResponse>
{
    public async Task<Result<GetUserByIdQueryResponse>> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var userResult = await userRepository.GetUserByIdAsync(new(request.Id), cancellationToken);
        if (userResult.IsFailure) return Result.Failure<GetUserByIdQueryResponse>(userResult.Errors);

        var user = userResult.Value.User;
        if (user == null) return Result.Success<GetUserByIdQueryResponse>(null);

        return Result.Success<GetUserByIdQueryResponse>(new(
            Id: user.Id,
            UserName: user.UserName,
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName
        ));
    }
}

