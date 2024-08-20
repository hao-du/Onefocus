using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class Currency : WriteEntityBase
{
    public string Name { get; private set; }
    public string ShortName { get; private set; }

    private Currency(string name, string shortName, string description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Name = name;
        ShortName = shortName;
    }

    public static Result<Currency> Create(string name, string shortName, string description, Guid actionedBy)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<Currency>(Errors.Currency.NameRequired);
        }
        if (string.IsNullOrEmpty(shortName))
        {
            return Result.Failure<Currency>(Errors.Currency.ShortNameRequired);
        }

        return new Currency(name, shortName, description, actionedBy);
    }

    public Result<Currency> Update(string name, string shortName, string description, bool activeFlag, Guid actionedBy)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<Currency>(Errors.Currency.NameRequired);
        }
        if (string.IsNullOrEmpty(shortName))
        {
            return Result.Failure<Currency>(Errors.Currency.ShortNameRequired);
        }

        Name = name;
        ShortName = shortName;
        Description = description;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return this;
    }
}