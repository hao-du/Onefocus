using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using static Onefocus.Wallet.Domain.Errors;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

public sealed class BankAccount : BaseTransaction
{
    public decimal Amount { get; init; }
    public Guid CurrencyId { get; init; }
    public decimal? InterestRate { get; init; }
    public string AccountNumber { get; init; } = default!;

    public DateTimeOffset IssuedOn { get; init; }
    public DateTimeOffset ClosedOn { get; init; }
    public bool CloseFlag { get; init; }
    public Guid BankId { get; init; }

    public Bank Bank { get; init; } = default!;
    public Currency Currency { get; init; } = default!;
}

