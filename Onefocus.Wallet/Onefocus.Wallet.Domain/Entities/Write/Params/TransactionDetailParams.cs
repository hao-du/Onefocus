using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Write.Params
{
    public class TransactionDetailParams
    {
        public TransactionDetailParams(Guid id, decimal amount, DateTimeOffset transactedOn, Enums.Action action, Guid transactionId, string? description, bool isActive, Guid actionedBy)
        {
            Id = id;
            Amount = amount;
            TransactedOn = transactedOn;
            Action = action;
            TransactionId = transactionId;
            Description = description;
            isActive = isActive;
            ActionedBy = actionedBy;
        }

        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public DateTimeOffset TransactedOn { get; private set; }
        public Enums.Action Action { get; private set; }
        public Guid TransactionId { get; private set; }
        public string? Description { get; private set; }
        public bool isActive { get; private set; }
        public Guid ActionedBy { get; private set; }
    }
}
