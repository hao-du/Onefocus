﻿using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

public sealed class CashFlow : ReadEntityBase
{
    public Guid TransactionId { get; init; }
    public bool IsIncome { get; init; }


    public Transaction Transaction { get; init; } = default!;
}

