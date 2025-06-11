using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Abstractions.Domain.Fields;
using Onefocus.Wallet.Domain.Entities.Interfaces;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Read;

public class Currency : ReadEntityBase, INameField, IOwnerUserField
{
    private readonly List<Transaction> _transactions = [];
    private readonly List<BankAccount> _bankAccounts = [];

    public string Name { get; init; } = default!;
    public string ShortName { get; init; } = default!;
    public bool IsDefault { get; init; }
    public Guid OwnerUserId { get; init; }

    public User OwnerUser { get; init; } = default!;
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();
}