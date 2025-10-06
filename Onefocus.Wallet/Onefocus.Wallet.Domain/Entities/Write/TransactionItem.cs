using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class TransactionItem : WriteEntityBase
{
    public Guid TransactionId { get; private set; }
    public string Name { get; private set; } = default!;
    public decimal Amount { get; private set; }

    public Transaction Transaction { get; private set; } = default!;

    private TransactionItem()
    {
        // Required for EF Core
    }

    private TransactionItem(string name, decimal amount, string? description, Guid actionedBy, Guid? transactionId = null)
    {
        Init(Guid.Empty, description, actionedBy);

        if (transactionId.HasValue)
        {
            TransactionId = transactionId.Value;
        }
        Name = name;
        Amount = amount;
    }

    public static Result<TransactionItem> Create(string name, decimal amount, string? description, Guid actionedBy, Guid? transactionId = null)
    {
        var validationResult = Validate(name, amount);
        if (validationResult.IsFailure) return (Result<TransactionItem>)validationResult;

        return new TransactionItem(name, amount, description, actionedBy, transactionId);
    }

    public Result Update(string name, decimal amount, string? description, bool isActive, Guid actionedBy)
    {
        var validationResult = Validate(name, amount);
        if (validationResult.IsFailure)
        {
            return Result.Failure(validationResult.Error);
        }

        Name = name;
        Amount = amount;
        Description = description;

        SetActiveFlag(isActive, actionedBy);

        return Result.Success();
    }

    public static Result Validate(string name, decimal amount)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure(Errors.TransactionItem.ItemNameRequired);
        }
        if (amount < 0)
        {
            return Result.Failure(Errors.Transaction.AmountMustEqualOrGreaterThanZero);
        }
        if (amount > 10000000000)
        {
            return Result.Failure(Errors.Transaction.AmountMustEqualOrLessThanTenBillion);
        }

        return Result.Success();
    }
}

