using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Wallet.Domain.Entities.Read;
using static Onefocus.Wallet.Infrastructure.Repositories.Read.GetAllUsersRepositoryResponse;

namespace Onefocus.Wallet.Infrastructure.Repositories.Read;

public sealed record GetAllUsersRepositoryResponse(List<UserResponse> Users) : ICastObject<GetAllUsersRepositoryResponse, List<User>>
{
    public static GetAllUsersRepositoryResponse Cast(List<User> source)
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
public sealed record GetUserByIdRepositoryResponse(Guid Id, string Email, string FirstName, string LastName, bool ActiveFlag) : ICastObject<GetUserByIdRepositoryResponse?, User>
{
    public static GetUserByIdRepositoryResponse? Cast(User? source)
    {
        if (source == null) return null;
        return new(source.Id, source.Email, source.FirstName, source.LastName, source.ActiveFlag);
    }
}

