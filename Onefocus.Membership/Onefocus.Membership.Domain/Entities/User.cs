using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain.ValueObjects;
using System.Collections.ObjectModel;

namespace Onefocus.Membership.Domain.Entities;

public class User : IdentityUser<Guid>
{
    private readonly List<UserRole> _userRoles = new();
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public virtual IReadOnlyCollection<UserRole> UserRoles => _userRoles;

    private User(string userName, string firstName, string lastName) : base(userName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<User> Create(UserCommandObject createCommandObject)
    {
        return new User(createCommandObject.Email, createCommandObject.FirstName, createCommandObject.LastName);
    }

    public void Update(UserCommandObject valueObject)
    {
        Email = valueObject.Email;
        FirstName = valueObject.FirstName;
        LastName = valueObject.LastName;
    }

    public Result Update(PasswordCommandObject valueObject, IPasswordHasher<User> passwordHash)
    {
        if (passwordHash == null)
        {
            return Result.Failure(CommonErrors.NullReference);
        }

        if (valueObject.Id == Guid.Empty)
        {
            return Result.Failure(Errors.User.IdRequired);
        }

        PasswordHash = passwordHash.HashPassword(this, valueObject.Password);

        return Result.Success();
    }
}

