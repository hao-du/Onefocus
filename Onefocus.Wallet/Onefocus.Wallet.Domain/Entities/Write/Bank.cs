using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;
using System.Security.Cryptography.X509Certificates;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class Bank : WriteEntityBase
{
    public string Name { get; private set; }

    private Bank(string name, string description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Name = name;
    }

    public static Result<Bank> Create(string name, string description, Guid actionedBy)
    {
        var validationResult = Validate(name);
        if (validationResult.IsFailure)
        {
            return Result.Failure<Bank>(validationResult.Error);
        }

        return new Bank(name, description, actionedBy);
    }

    public Result Update(string name, string description, bool activeFlag, Guid actionedBy)
    {
        var validationResult = Validate(name);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Description = description;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return Result.Success();
    }

    private static Result Validate(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<Bank>(Errors.Bank.NameRequired);
        }

        return Result.Success();
    }
}