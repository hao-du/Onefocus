using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Params;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class CurrencyExchange : BaseTransaction, IAggregateRoot
{
    public Guid BaseCurrencyId { get; private set; }
    public Guid TargetCurrencyId { get; private set; }
    public decimal ExchangeRate { get; private set; }


    public Currency BaseCurrency { get; private set; } = default!;
    public Currency TargetCurrency { get; private set; } = default!;

    private CurrencyExchange() : base()
    {
    }

    public CurrencyExchange(Guid baseCurrencyId, Guid targetCurrencyId, decimal exchangeRate, string? description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        BaseCurrencyId = baseCurrencyId;
        TargetCurrencyId = targetCurrencyId;
        ExchangeRate = exchangeRate;
    }

    public static Result<CurrencyExchange> Create(Guid baseCurrencyId, Guid targetCurrencyId, decimal exchangeRate, string? description, Guid actionedBy)
    {
        var validationResult = Validate(baseCurrencyId, targetCurrencyId, exchangeRate);
        if (validationResult.IsFailure)
        {
            return Result.Failure<CurrencyExchange>(validationResult.Errors);
        }

        return new CurrencyExchange(baseCurrencyId, targetCurrencyId, exchangeRate, description, actionedBy);
    }

    protected override Result CreateTransaction(decimal amount, DateTimeOffset transactedOn, Guid currencyId, string? description, Guid actionedBy, IReadOnlyList<TransactionItemParams>? transactionItems = null)
    {
        if (_transactions.Any(t => t.CurrencyId == currencyId))
        {
            return Result.Failure(Errors.Transaction.InvalidTransaction);
        }

        return base.CreateTransaction(amount, transactedOn, currencyId, description, actionedBy);
    }

    public Result CreateExchangeTransaction(decimal baseAmount, decimal targetAmount, DateTimeOffset transactedOn, string? description, Guid actionedBy)
    {
        var createBaseTransactionResult = CreateTransaction(baseAmount, transactedOn, BaseCurrencyId, description, actionedBy);
        if (createBaseTransactionResult.IsFailure)
        {
            return createBaseTransactionResult;
        }

        var createTargetTransactionResult = CreateTransaction(targetAmount, transactedOn, TargetCurrencyId, description, actionedBy);
        if (createTargetTransactionResult.IsFailure)
        {
            return createTargetTransactionResult;
        }

        return Result.Success();
    }

    public Result Update(Guid baseCurrencyId, Guid targetCurrencyId, decimal exchangeRate, bool isActive, string? description, Guid actionedBy, IReadOnlyList<TransactionParams> transactions)
    {
        var validationResult = Validate(baseCurrencyId, targetCurrencyId, exchangeRate);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        BaseCurrencyId = baseCurrencyId;
        TargetCurrencyId = targetCurrencyId;
        ExchangeRate = exchangeRate;
        Description = description;

        if (isActive) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        foreach (var transaction in transactions)
        {
            UpsertTransaction(transaction, actionedBy);
        }

        return Result.Success();
    }

    private static Result Validate(Guid baseCurrencyId, Guid targetCurrencyId, decimal exchangeRate)
    {
        if (exchangeRate <= 0)
        {
            return Result.Failure(Errors.CurrencyExchange.BaseCurrencyRequired);
        }
        if (baseCurrencyId == default)
        {
            return Result.Failure(Errors.CurrencyExchange.BaseCurrencyRequired);
        }
        if (targetCurrencyId == default)
        {
            return Result.Failure(Errors.CurrencyExchange.TargetCurrencyRequired);
        }

        return Result.Success();
    }
}

