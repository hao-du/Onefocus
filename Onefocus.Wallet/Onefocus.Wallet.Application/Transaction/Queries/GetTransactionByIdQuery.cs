using Onefocus.Common.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Application.Transaction.Queries
{
    public sealed record GetTransactionByIdQueryRequest(Guid CurrencyId, DateTimeOffset TransactedOn, string? Description) : IQuery;
}
