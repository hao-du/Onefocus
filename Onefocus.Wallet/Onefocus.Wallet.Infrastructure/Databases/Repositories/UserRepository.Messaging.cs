using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain.Entities;
using static Onefocus.Wallet.Infrastructure.Databases.Repositories.GetAllUsersRepositoryResponse;
using Entity = Onefocus.Membership.Domain.Entities;

namespace Onefocus.Wallet.Infrastructure.Databases.Repositories;

public sealed record UpsertUserRepositoryRequest(Guid Id, string Email, string FirstName, string LastName, bool ActionFlag, Guid ActionBy) : IRequestObject<Result<User>>
{
    public Result<User> ToRequestObject() => User.Create(Email, FirstName, LastName, ActionBy);
}
public sealed record UpsertUserRepositoryResponse(Guid Id);

public sealed record GetAllUsersRepositoryResponse(List<UserResponse> Users) : IResponseObject<GetAllUsersRepositoryResponse, List<User>>
{
    public static GetAllUsersRepositoryResponse Create(List<User> source)
    {
        var users = new List<UserResponse>();
        if (source != null && source.Any())
        {
            users = source.Select(s => new UserResponse(s.Id, s.Email, s.FirstName, s.LastName, s.ActiveFlag)).ToList();
        }

        return new(users);
    }

    public sealed record UserResponse(Guid Id, string Email, string FirstName, string LastName, bool ActiveFlag);
}

public sealed record GetUserByIdRepositoryRequest(Guid Id);
public sealed record GetUserByIdRepositoryResponse(Guid Id, string Email, string FirstName, string LastName, bool ActiveFlag) : IResponseObject<GetUserByIdRepositoryResponse?, User>
{
    public static GetUserByIdRepositoryResponse? Create(User? source) 
    {
        if (source == null) return null;
        return new(source.Id, source.Email, source.FirstName, source.LastName, source.ActiveFlag);
    } 
}

