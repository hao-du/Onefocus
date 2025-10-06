using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Params;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class CurrencyExchange : WriteEntityBase, IAggregateRoot
{
    private readonly List<CurrencyExchangeTransaction> _currencyExchangeTransactions = [];

    public decimal ExchangeRate { get; private set; }

    public IReadOnlyCollection<CurrencyExchangeTransaction> CurrencyExchangeTransactions => _currencyExchangeTransactions.AsReadOnly();

    private CurrencyExchange()
    {
        // Required by EF Core
    }

    private CurrencyExchange(decimal exchangeRate, string? description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        ExchangeRate = exchangeRate;
    }

    public static Result<CurrencyExchange> Create(CurrencyExchangeParams source, CurrencyExchangeParams target, decimal exchangeRate, DateTimeOffset transactedOn, string? description, Guid ownerId, Guid actionedBy)
    {
        var validationResult = Validate(source, target, exchangeRate);
        if (validationResult.IsFailure) return (Result<CurrencyExchange>)validationResult;

        var exchange = new CurrencyExchange(exchangeRate, description, actionedBy);

        var createSourceTransactionResult = Transaction.Create(
            amount: source.Amount,
            transactedOn: transactedOn,
            currencyId: source.CurrencyId,
            description: description,
            ownerId: ownerId,
            actionedBy: actionedBy
        );
        if (createSourceTransactionResult.IsFailure) return createSourceTransactionResult.Failure<CurrencyExchange>();
        var createExchangeSourceTransactionResult = CurrencyExchangeTransaction.Create(
            transaction: createSourceTransactionResult.Value,
            isTarget: false
        );
        if (createExchangeSourceTransactionResult.IsFailure) return createExchangeSourceTransactionResult.Failure<CurrencyExchange>();

        var createTargetTransactionResult = Transaction.Create(
            amount: target.Amount,
            transactedOn: transactedOn,
            currencyId: target.CurrencyId,
            description: description,
            ownerId: ownerId,
            actionedBy: actionedBy
        );
        if (createTargetTransactionResult.IsFailure) return createTargetTransactionResult.Failure<CurrencyExchange>();
        var createExchangeTargetTransactionResult = CurrencyExchangeTransaction.Create(
            transaction: createTargetTransactionResult.Value,
            isTarget: true
        );
        if (createExchangeTargetTransactionResult.IsFailure) return createExchangeTargetTransactionResult.Failure<CurrencyExchange>();

        exchange._currencyExchangeTransactions.Add(createExchangeSourceTransactionResult.Value);
        exchange._currencyExchangeTransactions.Add(createExchangeTargetTransactionResult.Value);

        return Result.Success(exchange);
    }

    public Result Update(CurrencyExchangeParams source, CurrencyExchangeParams target, decimal exchangeRate, DateTimeOffset transactedOn, string? description, bool isActive, Guid actionedBy)
    {
        var validationResult = Validate(source, target, exchangeRate);
        if (validationResult.IsFailure) return (Result<CurrencyExchange>)validationResult;

        ExchangeRate = exchangeRate;
        Description = description;
        SetActiveFlag(isActive, actionedBy);

        var sourceExchangeTransaction = _currencyExchangeTransactions.Find(t => !t.IsTarget);
        var targetExchangeTransaction = _currencyExchangeTransactions.Find(t => t.IsTarget);
        var sourceTransaction = sourceExchangeTransaction?.Transaction;
        var targetTransaction = targetExchangeTransaction?.Transaction;
        if (sourceExchangeTransaction == null || targetExchangeTransaction == null || sourceTransaction == null || targetTransaction == null)
        {
            return Result.Failure(Errors.Transaction.InvalidTransaction);
        }

        sourceExchangeTransaction.SetActiveFlag(isActive, actionedBy);
        targetExchangeTransaction.SetActiveFlag(isActive, actionedBy);

        var updateSourceTransactionResult = sourceTransaction.Update(
            amount: source.Amount,
            transactedOn: transactedOn,
            currencyId: source.CurrencyId,
            isActive: isActive,
            description: description,
            actionedBy: actionedBy
        );
        if (updateSourceTransactionResult.IsFailure) return updateSourceTransactionResult;

        var updateTargetTransactionResult = targetTransaction.Update(
            amount: target.Amount,
            transactedOn: transactedOn,
            currencyId: target.CurrencyId,
            isActive: isActive,
            description: description,
            actionedBy: actionedBy
        );
        if (updateTargetTransactionResult.IsFailure) return updateTargetTransactionResult;

        return Result.Success();
    }

    public static Result Validate(CurrencyExchangeParams source, CurrencyExchangeParams target, decimal exchangeRate)
    {
        if (exchangeRate < 1)
        {
            return Result.Failure(Errors.CurrencyExchange.ExchangeRateMustEqualOrGreaterThanOne);
        }
        if (exchangeRate > 100)
        {
            return Result.Failure(Errors.CurrencyExchange.ExchangeRateMustEqualOrLessThanOneHundred);
        }
        if (source.Amount < 0)
        {
            return Result.Failure(Errors.Transaction.AmountMustEqualOrGreaterThanZero);
        }
        if (source.Amount > 10000000000)
        {
            return Result.Failure(Errors.Transaction.AmountMustEqualOrLessThanTenBillion);
        }
        if (source.CurrencyId == default)
        {
            return Result.Failure(Errors.CurrencyExchange.SourceCurrencyRequired);
        }
        if (target.Amount < 0)
        {
            return Result.Failure(Errors.Transaction.AmountMustEqualOrGreaterThanZero);
        }
        if (target.Amount > 10000000000)
        {
            return Result.Failure(Errors.Transaction.AmountMustEqualOrLessThanTenBillion);
        }
        if (target.CurrencyId == default)
        {
            return Result.Failure(Errors.CurrencyExchange.TargetCurrencyRequired);
        }

        return Result.Success();
    }
}

