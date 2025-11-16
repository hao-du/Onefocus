using Onefocus.Common.Abstractions.Domain;
using System.Text.Json;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Events.Transaction;

public class BankAccountUpsertedEvent: IDomainEvent<WriteEntity.BankAccount>
{
    public WriteEntity.BankAccount Entity { get; private set; }
    public string IndexName => nameof(WriteEntity.BankAccount);
    public string EntityId => Entity.Id.ToString();
    public string Payload { get; private set; } = default!;
    public string EventType => GetType().Name;

    private BankAccountUpsertedEvent(WriteEntity.BankAccount bankAccount)
    {
        Entity = bankAccount;
        Payload = JsonSerializer.Serialize(new
        {
            bankId = bankAccount.BankId,
            bankName = bankAccount.Bank.Name,
            currencyId = bankAccount.CurrencyId,
            currencyName = bankAccount.Currency.Name,
            accountNumber = bankAccount.AccountNumber,
            issuedOn = bankAccount.IssuedOn,
            closedOn = bankAccount.ClosedOn,
            ownerUserId = bankAccount.OwnerUserId,
            description = bankAccount.Description,
            isActive = bankAccount.IsActive,
            transactions = bankAccount.BankAccountTransactions.Select(bat => new
            {
                transactedOn = bat.Transaction.TransactedOn,
                description = bat.Transaction.Description,
                isActive = bat.Transaction.IsActive,
            }).ToArray()
        });
    }

    public static BankAccountUpsertedEvent Create(WriteEntity.BankAccount bankAccount)
    {
        return new(bankAccount);
    }
}