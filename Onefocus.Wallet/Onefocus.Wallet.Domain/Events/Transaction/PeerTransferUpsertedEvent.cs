using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Constants;
using Onefocus.Common.Utilities;
using Onefocus.Wallet.Domain.Constants;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Events.Transaction;

public class PeerTransferUpsertedEvents
{
    public static IDomainEvent AddSearchIndex(WriteEntity.PeerTransfer peerTransfer)
    {
        return DomainEvent.Create(
            eventType: DomainEventTypes.SearchIndex,
            objectName: SchemaNames.Counterparty,
            objectId: peerTransfer.Id.ToString(),
            payload: JsonHelper.SerializeJson(new
            {
                id = peerTransfer.Id,
                type = nameof(WriteEntity.CurrencyExchange),
                counterpartyId = peerTransfer.CounterpartyId,
                status = peerTransfer.Status,
                peerTransferType = peerTransfer.Type,
                description = peerTransfer.Description,
                isActive = peerTransfer.IsActive,
                transactions = peerTransfer.PeerTransferTransactions.Select(ptt => new
                {
                    id = ptt.Transaction.Id,
                    transactedOn = ptt.Transaction.TransactedOn,
                    currencyId = ptt.Transaction.CurrencyId,
                    isInFlow = ptt.IsInFlow,
                    description = ptt.Transaction.Description,
                    isActive = ptt.Transaction.IsActive,
                }).ToArray()
            }),
            keyValuePairs: []
        );
    }
}