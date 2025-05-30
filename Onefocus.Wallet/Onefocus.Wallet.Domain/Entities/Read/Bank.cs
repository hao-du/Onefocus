using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Read;

public sealed class Bank : ReadEntityBase
{
    private readonly List<BankAccount> _bankAccounts = [];

    public string Name { get; init; } = default!;

    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();
}