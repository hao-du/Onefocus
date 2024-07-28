using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain.ValueObjects;
using System.Collections.ObjectModel;

namespace Onefocus.Membership.Domain.Entities;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual User? User { get; private set; }
    public virtual Role? Role { get; private set; }

    private UserRole(User user, Role role)
    {
        User = user;
        UserId = user.Id;
        Role = role;
        RoleId = role.Id;
    }

    private UserRole(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    public static Result<UserRole> Create(UserRoleCommandObject valueObject)
    {
        if (valueObject.User == null)
        {
            return Result.Failure<UserRole>(Errors.UserRole.UserRequired);
        }
        if (valueObject.Role == null)
        {
            return Result.Failure<UserRole>(Errors.UserRole.RoleRequired);
        }

        return new UserRole(valueObject.User, valueObject.Role);
    }

    public static Result<UserRole> Create(UserIdRoleIdCommandObject valueObject)
    {
        if(valueObject.UserId == Guid.Empty)
        {
            return Result.Failure<UserRole>(Errors.UserRole.UserIdRequired);
        }
        if (valueObject.RoleId == Guid.Empty)
        {
            return Result.Failure<UserRole>(Errors.UserRole.RoleIdRequired);
        }

        return new UserRole(valueObject.UserId, valueObject.RoleId);
    }
}

