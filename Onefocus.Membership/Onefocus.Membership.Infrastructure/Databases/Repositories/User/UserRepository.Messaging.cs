using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Membership.Domain.Entities;
using Onefocus.Membership.Domain.ValueObjects;
using static Onefocus.Membership.Infrastructure.Databases.Repositories.User.GetAllUsersRepositoryResponse;

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

public sealed record GetAllUsersRepositoryResponse(List<UserReponse> Users)
{
    public sealed record UserReponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleRepsonse> Roles);

    public sealed record RoleRepsonse(Guid Id, string? RoleName) : IResponseObject<RoleRepsonse, Role>
    {
        public static RoleRepsonse Create(Role? source) => source != null ? new (source.Id, source.Name) : new(Guid.Empty, string.Empty);
    }
}

