using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Wallet.Domain.Entities.Write;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Read.Transactions;

public class TransferTransaction : Transaction
{
    public Guid TransferredUserID { get; init; }

    public User TransferredUser { get; init; } = default!;
}

