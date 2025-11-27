using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Constants;
using System.Text.Json;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Events.Transaction;

public class PeerTransferUpsertedEvent : IDomainEvent<WriteEntity.PeerTransfer>
{
    public WriteEntity.PeerTransfer Entity { get; private set; }
    public string IndexName => SchemaNames.Transaction;
    public string EntityId => Entity.Id.ToString();
    public object Payload { get; private set; }
    public string EventType => GetType().Name;

    private PeerTransferUpsertedEvent(WriteEntity.PeerTransfer peerTransfer)
    {
        Entity = peerTransfer;
        Payload = new
        {
            id = peerTransfer.Id,
            type = nameof(WriteEntity.CurrencyExchange),
            counterpartyId = peerTransfer.CounterpartyId,
            counterpartyName = peerTransfer.Counterparty.FullName,
            status = peerTransfer.Status,
            peerTransferType = peerTransfer.Type,
            description = peerTransfer.Description,
            isActive = peerTransfer.IsActive,
            transactions = peerTransfer.PeerTransferTransactions.Select(ptt => new
            {
                id = ptt.Transaction.Id,
                transactedOn = ptt.Transaction.TransactedOn,
                currencyId = ptt.Transaction.CurrencyId,
                currencyName = ptt.Transaction.Currency.Name,
                isInFlow = ptt.IsInFlow,
                description = ptt.Transaction.Description,
                isActive = ptt.Transaction.IsActive,
            }).ToArray()
        };
    }

    public static PeerTransferUpsertedEvent Create(WriteEntity.PeerTransfer peerTransfer)
    {
        return new(peerTransfer);
    }
}