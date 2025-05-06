using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Application.Transaction.Queries
{
    public sealed record GetAllTransactionsQueryRequest(PagingDto Paging) : IQuery;
}
