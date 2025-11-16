using Onefocus.Common.Abstractions.Domain;
using System.Text.Json;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Events.Transaction;

public class PeerTransferUpsertedEvent : IDomainEvent<WriteEntity.PeerTransfer>
{
    public WriteEntity.PeerTransfer Entity { get; private set; }
    public string IndexName => nameof(Entities.Write.Transaction);
    public string EntityId => Entity.Id.ToString();
    public string Payload { get; private set; } = default!;
    public string EventType => GetType().Name;

    private PeerTransferUpsertedEvent(WriteEntity.PeerTransfer peerTransfer)
    {
        Entity = peerTransfer;
        Payload = JsonSerializer.Serialize(new
        {
            type = nameof(WriteEntity.CurrencyExchange),
            counterpartyId = peerTransfer.CounterpartyId,
            counterpartyName = peerTransfer.Counterparty.FullName,
            status = peerTransfer.Status,
            peerTransferType = peerTransfer.Type,
            description = peerTransfer.Description,
            isActive = peerTransfer.IsActive,
            transactions = peerTransfer.PeerTransferTransactions.Select(ptt => new
            {
                transactedOn = ptt.Transaction.TransactedOn,
                currencyId = ptt.Transaction.CurrencyId,
                currencyName = ptt.Transaction.Currency.Name,
                isInFlow = ptt.IsInFlow,
                description = ptt.Transaction.Description,
                isActive = ptt.Transaction.IsActive,
            }).ToArray()
        }); ;
    }

    public static PeerTransferUpsertedEvent Create(WriteEntity.PeerTransfer peerTransfer)
    {
        return new(peerTransfer);
    }
}