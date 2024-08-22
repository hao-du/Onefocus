using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Wallet.Domain.Entities.Write;

public abstract class TransactionDetail: WriteEntityBase
{
    public decimal Amount { get; protected set; }
    public DateTimeOffset Date { get; protected set; }
    public Enums.Action Action { get; protected set; }

    protected TransactionDetail(decimal amount, DateTimeOffset date, Enums.Action action, string description, Guid actionedBy)
    {
        Amount = amount;
        Date = date;
        Action = action;

        Init(Guid.NewGuid(), description, actionedBy);
    }
}

