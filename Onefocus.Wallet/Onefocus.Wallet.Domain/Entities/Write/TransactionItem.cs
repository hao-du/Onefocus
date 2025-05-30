using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Models;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class TransactionItem : WriteEntityBase
{
    public Guid TransactionId { get; set; }
    public string Name { get; private set; }
    public decimal Amount { get; private set; }

    public Transaction Transaction { get; private set; } = default!;

    protected TransactionItem()
    {
        Name = default!;
    }

    protected TransactionItem(string name, decimal amount, string? description, Guid actionedBy, Guid? transactionId = null)
    {
        Init(Guid.NewGuid(), description, actionedBy);

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
        if (validationResult.IsFailure)
        {
            return Result.Failure<TransactionItem>(validationResult.Error);
        }

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

        if (isActive) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return Result.Success();
    }

    private static Result Validate(string name, decimal amount)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure(Errors.TransactionItem.ItemNameRequired);
        }
        if (amount < 0)
        {
            return Result.Failure(Errors.TransactionItem.ItemAmountMustGreaterThanZero);
        }

        return Result.Success();
    }
}

