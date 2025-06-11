using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Interfaces;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Read;

public class Transaction : ReadEntityBase, IOwnerUserField
{
    private readonly List<BankAccountTransaction> _bankAccountTransactions = [];
    private readonly List<PeerTransferTransaction> _peerTransferTransactions = [];
    private readonly List<CurrencyExchangeTransaction> _currencyExchangeTransactions = [];
    private readonly List<CashFlow> _cashFlows = [];
    private readonly List<TransactionItem> _transactionItems = [];

    public decimal Amount { get; private set; }
    public DateTimeOffset TransactedOn { get; protected set; }
    public Guid CurrencyId { get; protected set; }
    public Guid OwnerUserId { get; init; }

    public User OwnerUser { get; init; } = default!;
    public Currency Currency { get; protected set; } = default!;

    public IReadOnlyCollection<BankAccountTransaction> BankAccountTransactions => _bankAccountTransactions.AsReadOnly();
    public IReadOnlyCollection<PeerTransferTransaction> PeerTransferTransactions => _peerTransferTransactions.AsReadOnly();
    public IReadOnlyCollection<CurrencyExchangeTransaction> CurrencyExchangeTransactions => _currencyExchangeTransactions.AsReadOnly();
    public IReadOnlyCollection<CashFlow> CashFlows => _cashFlows.AsReadOnly();
    public IReadOnlyCollection<TransactionItem> TransactionItems => _transactionItems.AsReadOnly();
}

