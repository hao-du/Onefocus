using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public class ExchangeTransaction : Transaction
{
    public Guid ExchangedCurrencyId { get; private set; }
    public decimal ExchangeRate { get; private set; }

    private ExchangeTransaction(decimal amount, decimal exchangeRate, DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid exchangedCurrencyId, string description, Guid actionedBy) : base(amount, transactedOn, userId, currencyId, description, actionedBy)
    {
        ExchangedCurrencyId = exchangedCurrencyId;
        ExchangeRate = exchangeRate;
    }

    public static Result<ExchangeTransaction> Create(decimal amount, decimal exchangeRate, DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid exchangedCurrencyId, string description, Guid actionedBy)
    {
        var validationResult = Validate(amount, exchangeRate, transactedOn, userId, currencyId, exchangedCurrencyId);
        if (validationResult.IsFailure)
        {
            return Result.Failure<ExchangeTransaction>(validationResult.Error);
        }

        return new ExchangeTransaction(amount, exchangeRate, transactedOn, userId, exchangedCurrencyId, currencyId, description, actionedBy);
    }

    public Result Update(decimal amount, decimal exchangeRate, DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid exchangedCurrencyId, string description, bool activeFlag, Guid actionedBy)
    {
        var validationResult = Validate(amount, exchangeRate, transactedOn, userId, currencyId, exchangedCurrencyId);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Amount = amount;
        TransactedOn = transactedOn;
        UserId = userId;
        ExchangedCurrencyId = exchangedCurrencyId;
        ExchangeRate = exchangeRate;
        CurrencyId = currencyId;
        Description = description;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return Result.Success();
    }

    private static Result Validate(decimal amount, decimal exchangeRate, DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid exchangedCurrencyId)
    {
        if (amount < 0)
        {
            return Result.Failure(Errors.Transaction.AmountMustGreaterThanZero);
        }
        if (exchangeRate < 0)
        {
            return Result.Failure<ExchangeTransaction>(Errors.Transaction.Exchange.ExchangeRateMustGreaterThanZero);
        }
        if (userId == Guid.Empty)
        {
            return Result.Failure(Errors.User.UserRequired);
        }
        if (currencyId == Guid.Empty)
        {
            return Result.Failure(Errors.Currency.CurrencyRequired);
        }
        if (exchangedCurrencyId == Guid.Empty)
        {
            return Result.Failure<ExchangeTransaction>(Errors.Transaction.Exchange.ExchangedCurrencyRequired);
        }

        return Result.Success();
    }
}

