using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class User : WriteEntityBase
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    private User(string email, string firstName, string lastName, string description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Result<User> Create(string email, string firstName, string lastName, string description, Guid actionedBy)
    {
        var validationResult = Validate(email, firstName, lastName);
        if (validationResult.IsFailure)
        {
            return Result.Failure<User>(validationResult.Error);
        }

        return new User(email, firstName, lastName, description, actionedBy);
    }

    public Result<User> Update(string email, string firstName, string lastName, string description, bool activeFlag, Guid actionedBy)
    {
        var validationResult = Validate(email, firstName, lastName);
        if (validationResult.IsFailure)
        {
            return Result.Failure<User>(validationResult.Error);
        }

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Description = description;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return this;
    }
    private static Result Validate(string email, string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<User>(Errors.User.EmailRequired);
        }
        if (string.IsNullOrEmpty(firstName))
        {
            return Result.Failure<User>(Errors.User.FirstNameRequired);
        }
        if (string.IsNullOrEmpty(lastName))
        {
            return Result.Failure<User>(Errors.User.LastNameRequired);
        }

        return Result.Success();
    }
}