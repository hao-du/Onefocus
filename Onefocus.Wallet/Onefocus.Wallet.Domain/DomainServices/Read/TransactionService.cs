using Onefocus.Common.Utilities;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Read;
using System.ComponentModel;

namespace Onefocus.Wallet.Domain.DomainServices.Read;

public class TransactionService()
{
    public TransactionType GetTransactionType(Transaction transaction)
    {
        if (transaction.BankAccounts.Count > 0)
            return TransactionType.BankAccount;
        if (transaction.PeerTransfers.Count > 0)
            return TransactionType.PeerTransfer;
        if (transaction.CurrencyExchanges.Count > 0)
            return TransactionType.CurrencyExchange;

        return TransactionType.CashFlow;
    }

    public IReadOnlyList<string> GetTransactionTag(Transaction transaction)
    {
        var type = GetTransactionType(transaction);
        var tags = new List<string>();

        switch (type)
        {
            case TransactionType.BankAccount:
                tags.Add("Interest");
                break;
            case TransactionType.PeerTransfer:
                var peerTransfer = transaction.PeerTransfers.First();
                var transferType = peerTransfer.Type.GetAttribute<DescriptionAttribute>()?.Description;
                var transferStatus = peerTransfer.Status.GetAttribute<DescriptionAttribute>()?.Description;

                if (!string.IsNullOrEmpty(transferType))
                    tags.Add(transferType);

                tags.Add(peerTransfer.TransferredUser.GetFullName());

                if (!string.IsNullOrEmpty(transferStatus))
                    tags.Add(transferStatus);

                break;
            case TransactionType.CurrencyExchange:
                var exchange = transaction.CurrencyExchanges.First();
                break;

            case TransactionType.CashFlow:
                break;
            default:
                return new List<string>();
        }
    }
}

