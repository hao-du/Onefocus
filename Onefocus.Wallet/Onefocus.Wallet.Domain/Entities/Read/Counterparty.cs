﻿using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Interfaces;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Read;

public sealed class Counterparty : ReadEntityBase, IOwnerUserField
{
    private readonly List<PeerTransfer> _peerTransfers = [];

    public string FullName { get; init; } = default!;
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public Guid OwnerUserId { get; init; }

    public User OwnerUser { get; init; } = default!;

    public IReadOnlyCollection<PeerTransfer> PeerTransfers => _peerTransfers.AsReadOnly();
}