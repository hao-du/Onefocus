using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

public sealed class CurrencyExchange : BaseTransaction
{
    public Guid BaseCurrencyId { get; init; }
    public Guid TargetCurrencyId { get; init; }
    public decimal ExchangeRate { get; init; }


    public Currency BaseCurrency { get; init; } = default!;
    public Currency TargetCurrency { get; init; } = default!;
}

