using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Membership.Domain.ValueObjects;

namespace Onefocus.Membership.Infrastructure.Databases.Repositories.User;

public record CommandUserRepositoryRequest(string Email, string FirstName, string LastName) : IRequestObject<UserCommandObject>
{
    public UserCommandObject ToRequestObject() => new(Email, FirstName, LastName);
}
public sealed record CreateUserRepositoryRequest(string Email, string FirstName, string LastName, string Password) 
    : CommandUserRepositoryRequest(Email, FirstName, LastName);
public sealed record UpdateUserRepositoryRequest(Guid Id, string Email, string FirstName, string LastName) 
    : CommandUserRepositoryRequest(Email, FirstName, LastName);
public sealed record UpdatePasswordRepositoryRequest(Guid Id, string Password) : IRequestObject<PasswordCommandObject>
{
    public PasswordCommandObject ToRequestObject() => new(Id, Password);
}

public sealed record GetAllUsersUserItemRepositoryResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName);
public sealed record GetAllUsersRepositoryResponse(List<GetAllUsersUserItemRepositoryResponse> Users);