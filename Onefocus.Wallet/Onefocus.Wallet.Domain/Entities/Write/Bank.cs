using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Abstractions.Domain.Fields;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Interfaces;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Write;

public sealed class Bank : WriteEntityBase, INameField, IOwnerUserField, IAggregateRoot
{
    private readonly List<BankAccount> _bankAccounts = [];

    public string Name { get; private set; } = default!;
    public Guid OwnerUserId { get; private set; }

    public User OwnerUser { get; private set; } = default!;

    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();

    private Bank()
    {
        // Required for EF Core
    }

    private Bank(string name, string? description, Guid ownerId, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Name = name;
        OwnerUserId = ownerId;
    }

    public static Result<Bank> Create(string name, string? description, Guid ownerId, Guid actionedBy)
    {
        var validationResult = Validate(name);
        if (validationResult.IsFailure) return (Result<Bank>)validationResult;

        return new Bank(name, description, ownerId, actionedBy);
    }

    public Result Update(string name, string? description, bool isActive, Guid actionedBy)
    {
        var validationResult = Validate(name);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Name = name;
        Description = description;

        SetActiveFlag(isActive, actionedBy);

        return Result.Success();
    }

    public static Result Validate(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure(Errors.Bank.NameRequired);
        }

        return Result.Success();
    }
}