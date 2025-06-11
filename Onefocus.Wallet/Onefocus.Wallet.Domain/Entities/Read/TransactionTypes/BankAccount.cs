using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Interfaces;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

public sealed class BankAccount : ReadEntityBase, IOwnerUserField
{
    private readonly List<BankAccountTransaction> _bankAccountTransactions = [];

    public decimal Amount { get; init; }
    public Guid CurrencyId { get; init; }
    public decimal? InterestRate { get; init; }
    public string? AccountNumber { get; init; } = default!;

    public DateTimeOffset IssuedOn { get; init; }
    public DateTimeOffset? ClosedOn { get; init; }
    public bool CloseFlag { get; init; }
    public Guid BankId { get; init; }
    public Guid OwnerUserId { get; init; }

    public User OwnerUser { get; init; } = default!;
    public Bank Bank { get; init; } = default!;
    public Currency Currency { get; init; } = default!;

    public IReadOnlyCollection<BankAccountTransaction> BankAccountTransactions => _bankAccountTransactions.AsReadOnly();
}

