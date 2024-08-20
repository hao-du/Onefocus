using MediatR;
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Write;

public abstract class Transaction: WriteEntityBase
{
    public decimal Amount { get; protected set; }
    public DateTimeOffset TraceDate { get; protected set; }
    public Guid UserId { get; protected set; }
    public Guid CurrencyId { get; protected set; }
    public User? User { get; protected set; }
    public Currency? Currency { get; protected set; }

    protected Transaction(decimal amount, DateTimeOffset traceDate, Guid userId, Guid currencyId, string description, Guid actionedBy)
    {
        Amount = amount;
        TraceDate = traceDate;
        CurrencyId = currencyId;
        UserId = userId;

        Init(Guid.NewGuid (), description, actionedBy);
    }
}

