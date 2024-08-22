using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class Currency : WriteEntityBase
{
    public string Name { get; private set; }
    public string ShortName { get; private set; }
    public bool DefaultFlag { get; private set; }

    private Currency(string name, string shortName, string description, bool defaultFlag, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Name = name;
        ShortName = shortName;
        DefaultFlag = defaultFlag;
    }

    public static Result<Currency> Create(string name, string shortName, string description, bool defaultFlag, Guid actionedBy)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<Currency>(Errors.Currency.NameRequired);
        }
        if (string.IsNullOrEmpty(shortName))
        {
            return Result.Failure<Currency>(Errors.Currency.ShortNameRequired);
        }

        return new Currency(name, shortName, description, defaultFlag, actionedBy);
    }

    public Result<Currency> Update(string name, string shortName, string description, bool defaultFlag, bool activeFlag, Guid actionedBy)
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
        DefaultFlag = defaultFlag;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return this;
    }

    public void MarkDefault(bool defaultFlag, Guid actionedBy)
    {
        DefaultFlag = defaultFlag;
        Update(actionedBy);
    }
}