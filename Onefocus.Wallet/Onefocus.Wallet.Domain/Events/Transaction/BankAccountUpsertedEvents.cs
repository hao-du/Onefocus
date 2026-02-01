using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Constants;
using Onefocus.Common.Utilities;
using Onefocus.Wallet.Domain.Constants;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Events.Transaction;

public class BankAccountUpsertedEvents
{
    public static IDomainEvent AddSearchIndex(WriteEntity.BankAccount bankAccount)
    {
        return DomainEvent.Create(
            eventType: DomainEventTypes.SearchIndex,
            objectName: SchemaNames.Counterparty,
            objectId: bankAccount.Id.ToString(),
            payload: JsonHelper.SerializeJson(new
            {
                id = bankAccount.Id,
                type = nameof(WriteEntity.BankAccount),
                bankId = bankAccount.BankId,
                currencyId = bankAccount.CurrencyId,
                accountNumber = bankAccount.AccountNumber,
                issuedOn = bankAccount.IssuedOn,
                isClosed = bankAccount.IsClosed,
                closedOn = bankAccount.ClosedOn,
                ownerUserId = bankAccount.OwnerUserId,
                description = bankAccount.Description,
                isActive = bankAccount.IsActive,
                transactions = bankAccount.BankAccountTransactions.Select(bat => new
                {
                    id = bat.Transaction.Id,
                    transactedOn = bat.Transaction.TransactedOn,
                    description = bat.Transaction.Description,
                    isActive = bat.Transaction.IsActive,
                }).ToArray()
            }),
            keyValuePairs: []
        );
    }
}