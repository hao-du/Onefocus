using Onefocus.Common.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class Transaction: WriteEntityBase, IAggregateRoot
{
    public decimal Amount { get; private set; }
    public DateTimeOffset TraceDate { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
}

