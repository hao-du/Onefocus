using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Params;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class CurrencyExchange : WriteEntityBase, IAggregateRoot
{
    private readonly List<CurrencyExchangeTransaction> _currencyExchangeTransactions = [];

    public decimal ExchangeRate { get; init; }

    public IReadOnlyCollection<CurrencyExchangeTransaction> CurrencyExchangeTransactions => _currencyExchangeTransactions.AsReadOnly();

    public CurrencyExchange(Guid baseCurrencyId, Guid targetCurrencyId, decimal exchangeRate, string? description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        ExchangeRate = exchangeRate;
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

