using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class PeerTransferTransaction : WriteEntityBase
{
    public Guid PeerTransferId { get; private set; }
    public Guid TransactionId { get; private set; }
    public bool IsInFlow { get; private set; }

    public PeerTransfer PeerTransfer { get; private set; } = default!;
    public Transaction Transaction { get; private set; } = default!;

    private PeerTransferTransaction()
    {
        // Required for EF Core
    }

    private PeerTransferTransaction(Transaction transaction, bool isInFlow)
    {
        Transaction = transaction;
        IsInFlow = isInFlow;
    }

    public static Result<PeerTransferTransaction> Create(Transaction transaction, bool isInFlow)
    {
        return new PeerTransferTransaction(transaction, isInFlow);
    }

    public void Update(bool isInFlow, bool isActive, Guid actionedBy)
    {
        IsInFlow = isInFlow;

        SetActiveFlag(isActive, actionedBy);
    }
}

