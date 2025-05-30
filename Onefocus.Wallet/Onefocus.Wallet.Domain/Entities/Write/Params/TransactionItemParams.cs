using Onefocus.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Write.Params
{
    public class TransactionItemParams(Guid? id, string name, decimal amount, bool isActive, string? description)
    {
        public Guid? Id { get; private set; } = id;
        public string Name { get; private set; } = name;
        public decimal Amount { get; private set; } = amount;
        public string? Description { get; private set; } = description;
        public bool IsActive { get; private set; } = isActive;

        public static TransactionItemParams Create(Guid? id, string name, decimal amount, bool isActive, string? description)
        {
            return new TransactionItemParams(id, name, amount, isActive, description);
        }
    }
}
