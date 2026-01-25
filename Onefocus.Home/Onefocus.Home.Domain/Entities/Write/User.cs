using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;

namespace Onefocus.Home.Domain.Entities.Write;

public sealed class User : WriteEntityBase, IAggregateRoot
{
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string Email { get; private set; } = default!;

    public Settings Settings { get; private set; } = default!;

    private User()
    {
        // Required for EF Core
    }

    private User(Guid? id, string email, string firstName, string lastName, string? description, Guid actionedBy)
    {
        Init(description, actionedBy);

        Id = id ?? Guid.CreateVersion7();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Result<User> Create(Guid? id, string email, string firstName, string lastName, string? description, Guid actionedBy)
    {
        var validationResult = Validate(email, firstName, lastName);
        if (validationResult.IsFailure) return (Result<User>)validationResult;

        return new User(id, email, firstName, lastName, description, actionedBy);
    }

    public Result<User> Update(string email, string firstName, string lastName, string? description, bool isActive, Guid actionedBy)
    {
        var validationResult = Validate(email, firstName, lastName);
        if (validationResult.IsFailure)
        {
            return Result.Failure<User>(validationResult.Errors);
        }

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Description = description;

        SetActiveFlag(isActive, actionedBy);

        return this;
    }
    private static Result Validate(string email, string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure(Errors.User.EmailRequired);
        }
        if (string.IsNullOrEmpty(firstName))
        {
            return Result.Failure(Errors.User.FirstNameRequired);
        }
        if (string.IsNullOrEmpty(lastName))
        {
            return Result.Failure(Errors.User.LastNameRequired);
        }

        return Result.Success();
    }
}