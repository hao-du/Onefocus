using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Read;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Read.Transactions;

public class ExchangeTransaction : Transaction
{
    public Guid ExchangedCurrencyId { get; init; }
    public decimal ExchangeRate { get; init; }

    public Currency ExchangedCurrency { get; init; } = default!;
}

