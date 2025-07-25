﻿using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Abstractions.Domain.Fields;
using Onefocus.Wallet.Domain.Entities.Interfaces;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Read;

public sealed class Bank : ReadEntityBase, INameField, IOwnerUserField
{
    private readonly List<BankAccount> _bankAccounts = [];

    public string Name { get; init; } = default!;
    public Guid OwnerUserId { get; init; }

    public User OwnerUser { get; init; } = default!;
    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();
}