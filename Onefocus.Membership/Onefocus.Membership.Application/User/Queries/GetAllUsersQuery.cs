using MediatR;
using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.User.Services;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record GetAllUsersItemQueryResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName) 
    : IResponseObject<GetAllUsersItemQueryResponse, GetAllUsersItemServiceResponse>
{
    public static GetAllUsersItemQueryResponse Create(GetAllUsersItemServiceResponse source) 
        => new(source.Id, source.UserName, source.Email, source.FirstName, source.LastName);
}
public sealed record GetAllUsersQueryResponse(List<GetAllUsersItemQueryResponse> Users) 
    : IResponseObject<GetAllUsersQueryResponse, GetAllUsersServiceResponse>
{
    public static GetAllUsersQueryResponse Create(GetAllUsersServiceResponse source) 
        => new(source.Users.Select(user => GetAllUsersItemQueryResponse.Create(user)).ToList());
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

