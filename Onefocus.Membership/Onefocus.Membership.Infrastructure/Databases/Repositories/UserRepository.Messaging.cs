using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Membership.Domain.Entities;
using Onefocus.Membership.Domain.ValueObjects;
using Onefocus.Membership.Infrastructure.ServiceBus;
using static Onefocus.Membership.Infrastructure.Databases.Repositories.GetAllUsersRepositoryResponse;

namespace Onefocus.Membership.Infrastructure.Databases.Repositories;

public record CommandUserRepositoryRequest(string Email, string FirstName, string LastName)
{
    public UserCommandObject ToObject() => new(Email, FirstName, LastName);
}
public sealed record CreateUserRepositoryRequest(string Email, string FirstName, string LastName, string Password)
    : CommandUserRepositoryRequest(Email, FirstName, LastName);
public sealed record UpdateUserRepositoryRequest(Guid Id, string Email, string FirstName, string LastName)
    : CommandUserRepositoryRequest(Email, FirstName, LastName);
public sealed record UpdatePasswordRepositoryRequest(Guid Id, string Password)
{
    public PasswordCommandObject ToObject() => new(Id, string.Empty, string.Empty, string.Empty, Password);
}
public sealed record UpdatePasswordRepositoryResponse(Guid Id, string Email, string FirstName, string LastName)
{
    public static UpdatePasswordRepositoryResponse CastFrom(User source) => new(source.Id, source.Email ?? string.Empty, source.FirstName, source.LastName);

    public IUserSyncedMessage ToObject(string encryptedPassword) => new UserSyncedPublishMessage(Id, Email, FirstName, LastName, null, true, encryptedPassword);
}

public sealed record GetAllUsersRepositoryResponse(List<UserReponse> Users)
{
    public sealed record UserReponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleRepsonse> Roles)
    {
        public IUserSyncedMessage ToObject() => new UserSyncedPublishMessage(Id, Email ?? string.Empty, FirstName, LastName, null, true, null);
    }

    public sealed record RoleRepsonse(Guid Id, string? RoleName)
    {
        public static RoleRepsonse CastFrom(Role? source) => source != null ? new(source.Id, source.Name) : new(Guid.Empty, string.Empty);
    }
}

public sealed record GetUserByIdRepositoryRequest(Guid Id);
public sealed record GetUserByIdRepositoryResponse(GetUserByIdRepositoryResponse.UserResponse User)
{
    public sealed record UserResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleResponse> Roles);
    public sealed record RoleResponse(Guid Id, string? RoleName);
    public static GetUserByIdRepositoryResponse CastFrom(User source)
    {
        var user = new UserResponse(
            source.Id
            , source.UserName
            , source.Email
            , source.FirstName
            , source.LastName
            , [.. source.UserRoles.Select(r => r.Role != null ? new RoleResponse(r.Role.Id, r.Role.Name) : new RoleResponse(Guid.Empty, string.Empty))]
        );

        return new(user);
    }
}

public sealed record LockUserRepositoryRequest(Guid Id);

