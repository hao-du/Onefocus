using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Membership.Domain.Entities;
using Onefocus.Membership.Domain.ValueObjects;
using Entity = Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Infrastructure.Databases.Repositories.User;

public record CommandUserRepositoryRequest(string Email, string FirstName, string LastName): IRequestObject<UserCommandObject>
{
    public UserCommandObject ToRequestObject() => new(Email, FirstName, LastName);
}

public sealed record CreateUserRepositoryRequest(string Email, string FirstName, string LastName, string Password): CommandUserRepositoryRequest(Email, FirstName, LastName);
public sealed record CreateUserRepositoryResponse(Guid Id);

public sealed record UpdateUserRepositoryRequest(Guid Id, string Email, string FirstName, string LastName): CommandUserRepositoryRequest(Email, FirstName, LastName);

public sealed record UpdatePasswordRepositoryRequest(Guid Id, string Password): IRequestObject<PasswordCommandObject>
{
    public PasswordCommandObject ToRequestObject() => new(Id, Password);
}

public sealed record GetAllUsersRepositoryResponse(List<GetAllUsersRepositoryResponse.UserResponse> Users): IResponseObject<GetAllUsersRepositoryResponse, List<Entity.User>>
{
    public sealed record UserResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleResponse> Roles);
    public sealed record RoleResponse(Guid Id, string? RoleName);
    public static GetAllUsersRepositoryResponse Create(List<Entity.User> source)
    {
        var users = source.Select(u => new UserResponse(
            u.Id
            , u.UserName
            , u.Email
            , u.FirstName
            , u.LastName
            , u.UserRoles.Select(r => r.Role != null ? new RoleResponse(r.Role.Id, r.Role.Name) : new RoleResponse(Guid.Empty, string.Empty)).ToList()
        )).ToList();

        return new(users);
    }
}

public sealed record GetUserByIdRepositoryRequest(Guid Id);
public sealed record GetUserByIdRepositoryResponse(GetUserByIdRepositoryResponse.UserResponse User) : IResponseObject<GetUserByIdRepositoryResponse, Entity.User>
{
    public sealed record UserResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleResponse> Roles);
    public sealed record RoleResponse(Guid Id, string? RoleName);
    public static GetUserByIdRepositoryResponse Create(Entity.User source)
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

