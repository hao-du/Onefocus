using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Abstractions.Domain.Fields;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;
using Onefocus.Wallet.Domain.Events.Currency;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class Currency : WriteEntityBase, INameField, IAggregateRoot
{
    private readonly List<Transaction> _transactions = [];
    private readonly List<BankAccount> _bankAccounts = [];

    public string Name { get; private set; } = default!;
    public string ShortName { get; private set; } = default!;
    public bool IsDefault { get; private set; }
    public Guid OwnerUserId { get; private set; }

    public User OwnerUser { get; private set; } = default!;

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();

    private Currency()
    {
        // Required for EF Core
    }

    private Currency(string name, string shortName, string? description, bool isDefault, Guid ownerId, Guid actionedBy)
    {
        Init(Guid.CreateVersion7(), description, actionedBy);

        Name = name;
        ShortName = shortName;
        IsDefault = isDefault;
        OwnerUserId = ownerId;
    }

    public static Result<Currency> Create(string name, string shortName, string? description, bool isDefault, Guid ownerId, Guid actionedBy)
    {
        var validationResult = Validate(name, shortName);
        if (validationResult.IsFailure) return (Result<Currency>)validationResult;

        var currency = new Currency(name, shortName, description, isDefault, ownerId, actionedBy);

        currency.AddDomainEvent(CurrencyUpsertedEvent.Create(currency));

        return currency;
    }

    public Result Update(string name, string shortName, string? description, bool isDefault, bool isActive, Guid actionedBy)
    {
        var validationResult = Validate(name, shortName);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Name = name;
        ShortName = shortName.ToUpper();
        Description = description;
        IsDefault = isDefault;

        SetActiveFlag(isActive, actionedBy);

        AddDomainEvent(CurrencyUpsertedEvent.Create(this));

        return Result.Success();
    }

    public static Result Validate(string name, string shortName)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure(Errors.Currency.NameRequired);
        }
        if (string.IsNullOrEmpty(shortName))
        {
            return Result.Failure(Errors.Currency.ShortNameRequired);
        }
        if (shortName.Length < 3 || shortName.Length > 4)
        {
            return Result.Failure(Errors.Currency.ShortNameLengthMustBeThreeOrFour);
        }

        return Result.Success();
    }
}