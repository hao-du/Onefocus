using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;

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
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<Bank>(Errors.Bank.NameRequired);
        }

        return new Bank(name, description, actionedBy);
    }

    public Result<Bank> Update(string name, string description, bool activeFlag, Guid actionedBy)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<Bank>(Errors.Bank.NameRequired);
        }

        Description = description;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return this;
    }
}