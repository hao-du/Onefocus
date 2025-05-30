using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

public sealed class CashFlow : BaseTransaction
{
    public CashFlowDirection Direction { get; init; }
}

