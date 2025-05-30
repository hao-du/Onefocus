using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Write.Models;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public sealed class CurrencyExchange : WriteEntityBase
{
    private List<Transaction> _transactions = new List<Transaction>();

    public Guid BaseCurrencyId { get; private set; }
    public Guid TargetCurrencyId { get; private set; }
    public decimal ExchangeRate { get; private set; }


    public Currency BaseCurrency { get; private set; } = default!;
    public Currency TargetCurrency { get; private set; } = default!;
    public IReadOnlyCollection<Transaction> TransactionDetails => _transactions.AsReadOnly();

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
            return Result.Failure<CurrencyExchange>(validationResult.Error);
        }

        return new CurrencyExchange(baseCurrencyId, targetCurrencyId, exchangeRate, description, actionedBy);
    }

    public Result Update(Guid baseCurrencyId, Guid targetCurrencyId, decimal exchangeRate, bool isActive, string? description, Guid actionedBy)
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

