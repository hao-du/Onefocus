namespace Onefocus.Wallet.Domain.Entities.Read.Transactions;

public sealed class ExchangeTransaction : Transaction
{
    public Guid ExchangedCurrencyId { get; init; }
    public decimal ExchangeRate { get; init; }

    public Currency ExchangedCurrency { get; init; } = default!;
}

