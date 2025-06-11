using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Read;

public sealed class User : ReadEntityBase
{
    private readonly List<Transaction> _transactions = [];
    private readonly List<BankAccount> _bankAccounts = [];
    private readonly List<Bank> _banks = [];
    private readonly List<Counterparty> _counterparties = [];
    private readonly List<Currency> _currencies = [];
    private readonly List<Option> _options = [];

    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();
    public IReadOnlyCollection<Bank> Banks => _banks.AsReadOnly();
    public IReadOnlyCollection<Counterparty> Counterparties => _counterparties.AsReadOnly();
    public IReadOnlyCollection<Currency> Currencies => _currencies.AsReadOnly();
    public IReadOnlyCollection<Option> Options => _options.AsReadOnly();

    public string GetFullName() => $"{FirstName} {LastName}".Trim();
}