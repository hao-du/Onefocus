using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Write.Models
{
    public class TransactionDetailParams
    {
        public TransactionDetailParams(Guid id, decimal amount, DateTimeOffset transactedOn, Enums.Action action, Guid transactionId, string? description, bool activeFlag, Guid actionedBy)
        {
            Id = id;
            Amount = amount;
            TransactedOn = transactedOn;
            Action = action;
            TransactionId = transactionId;
            Description = description;
            ActiveFlag = activeFlag;
            ActionedBy = actionedBy;
        }

        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public DateTimeOffset TransactedOn { get; private set; }
        public Enums.Action Action { get; private set; }
        public Guid TransactionId { get; private set; }
        public string? Description { get; private set; }
        public bool ActiveFlag { get; private set; }
        public Guid ActionedBy { get; private set; }
    }
}
