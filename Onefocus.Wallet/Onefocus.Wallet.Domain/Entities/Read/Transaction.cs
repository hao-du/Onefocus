﻿using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Utilities;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Interfaces;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;
using System.ComponentModel;

namespace Onefocus.Wallet.Domain.Entities.Read;

public class Transaction : ReadEntityBase, IOwnerUserField, IAggregateRoot
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

    public TransactionType GetTransactionType()
    {
        if (_bankAccountTransactions.Count > 0)
            return TransactionType.BankAccount;
        if (_peerTransferTransactions.Count > 0)
            return TransactionType.PeerTransfer;
        if (_currencyExchangeTransactions.Count > 0)
            return TransactionType.CurrencyExchange;

        return TransactionType.CashFlow;
    }

    public IReadOnlyList<string> GetTransactionTags()
    {
        var type = GetTransactionType();
        var tags = new List<string>();

        switch (type)
        {
            case TransactionType.BankAccount:
                tags.Add("Interest");
                break;
            case TransactionType.PeerTransfer:
                var peerTransferTransaction = _peerTransferTransactions.Single();
                var peerTransfer = peerTransferTransaction.PeerTransfer;
                if (peerTransfer == null) return [];

                switch (peerTransfer.Type)
                {
                    case PeerTransferType.Lend:
                        tags.Add(peerTransferTransaction.IsInFlow ? "Collection" : "Lending");
                        break;
                    case PeerTransferType.Borrow:
                        tags.Add(peerTransferTransaction.IsInFlow ? "Borrowing" : "Repayment");
                        break;
                    case PeerTransferType.Give:
                        tags.Add("Giving");
                        break;
                    case PeerTransferType.Receive:
                        tags.Add("Receiving");
                        break;
                }

                if (!string.IsNullOrEmpty(peerTransfer.Counterparty?.FullName))
                    tags.Add(peerTransfer.Counterparty.FullName);

                var transferStatus = peerTransfer.Status.GetAttribute<DescriptionAttribute>()?.Description;
                if (!string.IsNullOrEmpty(transferStatus))
                    tags.Add(transferStatus);
                break;
            case TransactionType.CurrencyExchange:
                var exchange = CurrencyExchangeTransactions.Single();
                tags.Add("Exchange");
                tags.Add(exchange.IsTarget ? "Target" : "Source");
                break;
            case TransactionType.CashFlow:
                var cashFlow = CashFlows.Single();
                tags.Add(cashFlow.IsIncome ? "Income" : "Expense");


                break;
        }

        return tags;
    }
}

