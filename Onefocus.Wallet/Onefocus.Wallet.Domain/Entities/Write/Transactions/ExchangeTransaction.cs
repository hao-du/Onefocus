using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public sealed class ExchangeTransaction : Transaction
{
    public Guid ExchangedCurrencyId { get; private set; }
    public decimal ExchangeRate { get; private set; }

    public Currency ExchangedCurrency { get; private set; } = default!;

    private ExchangeTransaction() : base()
    {
    }

    private ExchangeTransaction(decimal exchangeRate, DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid exchangedCurrencyId, string description, Guid actionedBy) : base(transactedOn, userId, currencyId, description, actionedBy)
    {
        ExchangedCurrencyId = exchangedCurrencyId;
        ExchangeRate = exchangeRate;
    }

    public static Result<ExchangeTransaction> Create(decimal exchangeRate, DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid exchangedCurrencyId, string description, Guid actionedBy, IReadOnlyList<ObjectValues.TransactionDetail> objectValueDetails)
    {
        var validationResult = Validate(exchangeRate, transactedOn, userId, currencyId, exchangedCurrencyId);
        if (validationResult.IsFailure)
        {
            return Result.Failure<ExchangeTransaction>(validationResult.Error);
        }

        var transaction = new ExchangeTransaction(exchangeRate, transactedOn, userId, exchangedCurrencyId, currencyId, description, actionedBy);

        foreach (var objectValueDetail in objectValueDetails)
        {
            var detailResult = transaction.AddDetail(objectValueDetail);
            if (detailResult.IsFailure)
            {
                return Result.Failure<ExchangeTransaction>(detailResult.Error);
            }
        }

        return transaction;
    }

    public Result Update(decimal amount, decimal exchangeRate, DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid exchangedCurrencyId, string description, bool activeFlag, Guid actionedBy, IReadOnlyList<ObjectValues.TransactionDetail> objectValueDetails)
    {
        var validationResult = Validate(exchangeRate, transactedOn, userId, currencyId, exchangedCurrencyId);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        TransactedOn = transactedOn;
        UserId = userId;
        ExchangedCurrencyId = exchangedCurrencyId;
        ExchangeRate = exchangeRate;
        CurrencyId = currencyId;
        Description = description;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        foreach (var objectValueDetail in objectValueDetails)
        {
            var detailResult = UpsertDetail(objectValueDetail);
            if (detailResult.IsFailure)
            {
                return Result.Failure(detailResult.Error);
            }
        }

        return Result.Success();
    }

    private static Result Validate(decimal exchangeRate, DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid exchangedCurrencyId)
    {
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

