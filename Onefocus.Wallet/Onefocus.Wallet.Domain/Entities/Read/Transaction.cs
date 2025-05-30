using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Onefocus.Wallet.Domain.Entities.Read;

public class Transaction : ReadEntityBase
{
    private readonly List<BankAccount> _bankAccounts = [];
    private readonly List<PeerTransfer> _peerTransfers = [];
    private readonly List<CurrencyExchange> _currencyExchanges = [];
    private readonly List<CashFlow> _cashFlows = [];
    private readonly List<TransactionItem> _transactionItems = [];

    public decimal Amount { get; private set; }
    public DateTimeOffset TransactedOn { get; protected set; }
    public Guid UserId { get; protected set; }
    public Guid CurrencyId { get; protected set; }

    public User User { get; protected set; } = default!;
    public Currency Currency { get; protected set; } = default!;

    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();
    public IReadOnlyCollection<PeerTransfer> PeerTransfers => _peerTransfers.AsReadOnly();
    public IReadOnlyCollection<CurrencyExchange> CurrencyExchanges => _currencyExchanges.AsReadOnly();
    public IReadOnlyCollection<CashFlow> CashFlows => _cashFlows.AsReadOnly();
    public IReadOnlyCollection<TransactionItem> TransactionItems => _transactionItems.AsReadOnly();
}

