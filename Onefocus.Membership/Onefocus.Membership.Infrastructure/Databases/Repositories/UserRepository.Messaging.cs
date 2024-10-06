using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Membership.Domain.Entities;
using Onefocus.Membership.Domain.ValueObjects;
using static Onefocus.Membership.Infrastructure.Databases.Repositories.GetAllUsersRepositoryResponse;
using Entity = Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Infrastructure.Databases.Repositories;

public record CommandUserRepositoryRequest(string Email, string FirstName, string LastName) : IToObject<UserCommandObject>
{
    public UserCommandObject ToObject() => new(Email, FirstName, LastName);
}
public sealed record CreateUserRepositoryRequest(string Email, string FirstName, string LastName, string Password)
    : CommandUserRepositoryRequest(Email, FirstName, LastName);
public sealed record UpdateUserRepositoryRequest(Guid Id, string Email, string FirstName, string LastName)
    : CommandUserRepositoryRequest(Email, FirstName, LastName);
public sealed record UpdatePasswordRepositoryRequest(Guid Id, string Password) : IToObject<PasswordCommandObject>
{
    public PasswordCommandObject ToObject() => new(Id, Password);
}

public sealed record GetAllUsersRepositoryResponse(List<UserReponse> Users)
{
    public sealed record UserReponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleRepsonse> Roles);

    public sealed record RoleRepsonse(Guid Id, string? RoleName) : ICastObject<RoleRepsonse, Role>
    {
        public static RoleRepsonse Cast(Role? source) => source != null ? new(source.Id, source.Name) : new(Guid.Empty, string.Empty);
    }
}

public sealed record GetUserByIdRepositoryRequest(Guid Id);
public sealed record GetUserByIdRepositoryResponse(GetUserByIdRepositoryResponse.UserResponse User) : ICastObject<GetUserByIdRepositoryResponse, User>
{
    public sealed record UserResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleResponse> Roles);
    public sealed record RoleResponse(Guid Id, string? RoleName);
    public static GetUserByIdRepositoryResponse Cast(User source)
    {
        var user = new UserResponse(
            source.Id
            , source.UserName
            , source.Email
            , source.FirstName
            , source.LastName
            , source.UserRoles.Select(r => r.Role != null ? new RoleResponse(r.Role.Id, r.Role.Name) : new RoleResponse(Guid.Empty, string.Empty)).ToList()
        );

        return new(user);
    }
}

public sealed record LockUserRepositoryRequest(Guid Id);

