using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Results;

namespace Onefocus.Membership.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    private User(string userName, string firstName, string lastName) : base(userName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<User> Create(string email, string firstName, string lastName)
    {
        var validationResult = Validate(email, firstName, lastName);
        if (validationResult.IsFailure) return (Result<User>)validationResult;

        return new User(email, firstName, lastName);
    }

    public Result Update(string email, string firstName, string lastName)
    {
        var validationResult = Validate(email, firstName, lastName);
        if (validationResult.IsFailure) return validationResult;

        Email = email;
        FirstName = firstName;
        LastName = lastName;

        return Result.Success();
    }

    public Result Update(string hashedPassword)
    {
        if (hashedPassword == null) return Result.Failure(Errors.User.PasswordRequired);
        if (Id == Guid.Empty) return Result.Failure(Errors.User.UserNotExist);

        PasswordHash = hashedPassword;

        return Result.Success();
    }

    private static Result Validate(string email, string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(email)) return Result.Failure(Errors.User.EmailRequired);
        if (string.IsNullOrEmpty(firstName)) return Result.Failure(Errors.User.FirstNameRequired);
        if (string.IsNullOrEmpty(lastName)) return Result.Failure(Errors.User.LastNameRequired);

        return Result.Success();
    }
}

