using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Abstractions.Domain.Fields;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Read;

public class Currency : ReadEntityBase, INameField
{
    private readonly List<Transaction> _transactions = [];
    private readonly List<BankAccount> _bankAccounts = [];
    private readonly List<CurrencyExchange> _baseCurrencyExchanges = [];
    private readonly List<CurrencyExchange> _targetCurrencyExchanges = [];

    public string Name { get; init; } = default!;
    public string ShortName { get; init; } = default!;
    public bool IsDefault { get; init; }

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();
    public IReadOnlyCollection<CurrencyExchange> BaseCurrencyExchanges => _baseCurrencyExchanges.AsReadOnly();
    public IReadOnlyCollection<CurrencyExchange> TargetCurrencyExchanges => _targetCurrencyExchanges.AsReadOnly();
}