using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;
using System.Collections.ObjectModel;

namespace Onefocus.Membership.Domain.Entities;

public class User: EntityBase, IAggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    private User(string email, string firstName, string lastName, Guid actionedBy)
    {
        Init(Guid.NewGuid(), actionedBy);

        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Result<User> Create(string email, string firstName, string lastName, Guid actionedBy)
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

        return new User(email, firstName, lastName, actionedBy);
    }

    public Result<User> Update(string email, string firstName, string lastName, bool activeFlag, Guid actionedBy)
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

        FirstName = firstName;
        LastName = lastName;
        Email = email;

        if (activeFlag) MarkActive(actionedBy); 
        else MarkInactive(actionedBy);

        return this;
    }
}

