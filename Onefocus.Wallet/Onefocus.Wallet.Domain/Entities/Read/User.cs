using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Read;

public sealed class User : ReadEntityBase
{
    private readonly List<Transaction> _transactions = [];
    private readonly List<PeerTransfer> _peerTransfers = [];

    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<PeerTransfer> PeerTransfers => _peerTransfers.AsReadOnly();
}