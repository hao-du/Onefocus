using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Write.Params;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class PeerTransfer : WriteEntityBase, IAggregateRoot
{
    private readonly List<PeerTransferTransaction> _peerTransferTransactions = [];

    public Guid CounterpartyId { get; init; }
    public PeerTransferStatus Status { get; init; }
    public PeerTransferType Type { get; init; }

    public Counterparty Counterparty { get; init; } = default!;

    public IReadOnlyCollection<PeerTransferTransaction> PeerTransferTransactions => _peerTransferTransactions.AsReadOnly();

    private PeerTransfer()
    {
        // Required for EF Core
    }

    private PeerTransfer(PeerTransferStatus status, PeerTransferType type, Guid counterpartyId, string? description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Status = status;
        Type = type;
        CounterpartyId = counterpartyId;
    }

    public static Result<PeerTransfer> Create(PeerTransferStatus status, PeerTransferType type, Guid counterpartyId, string? description, Guid ownerId, Guid actionedBy, Guid transferredUserId, IReadOnlyList<TransferTransactionParams> transactionParams)
    {
        var validationResult = Validate(transferredUserId);
        if (validationResult.IsFailure) return (Result<PeerTransfer>)validationResult;

        var peerTransfer = new PeerTransfer(status, type, counterpartyId, description, actionedBy);

        peerTransfer.UpsertTransferDetails(ownerId, actionedBy, transactionParams);

        return Result.Success(peerTransfer);
    }


    private Result UpsertTransferDetails(Guid ownerId, Guid actionedBy, IReadOnlyList<TransferTransactionParams> transactionParams)
    {
        foreach (var param in transactionParams)
        {
            var isNew = param.Id.HasValue && param.Id != Guid.Empty;
            if (isNew)
            {
                var createDetailResult = CreateTransferDetails(ownerId, actionedBy, param);
                if (createDetailResult.IsFailure) return createDetailResult;
            }
            else
            {
                var updateDetailResult = UpdateTransferDetails(actionedBy, param);
                if (updateDetailResult.IsFailure) return updateDetailResult;
            }
        }

        return Result.Success();
    }

    private Result CreateTransferDetails(Guid ownerId, Guid actionedBy, TransferTransactionParams param)
    {
        var createTransactionResult = Transaction.Create(
            amount: param.Amount,
            transactedOn: param.TransactedOn,
            currencyId: param.CurrencyId,
            description: param.Description,
            ownerId: ownerId,
            actionedBy: actionedBy,
            transactionItems: param.TransactionItems
        );
        if (createTransactionResult.IsFailure) return createTransactionResult;

        var createPeerTransferTransactionResult = PeerTransferTransaction.Create(createTransactionResult.Value, param.IsInFlow);
        if (createPeerTransferTransactionResult.IsFailure) return createPeerTransferTransactionResult;

        _peerTransferTransactions.Add(createPeerTransferTransactionResult.Value);

        return Result.Success();
    }

    private Result UpdateTransferDetails(Guid actionedBy, TransferTransactionParams param)
    {
        var peerTransferTransaction = _peerTransferTransactions.Find(bat => bat.TransactionId == param.Id);
        if (peerTransferTransaction == null)
        {
            return Result.Failure(Errors.Transaction.InvalidTransaction);
        }

        peerTransferTransaction.Update(isInFlow: param.IsInFlow, param.IsActive, actionedBy);

        var updateResult = peerTransferTransaction.Transaction.Update(param.Amount, param.TransactedOn, param.CurrencyId, param.IsActive, param.Description, actionedBy);
        if (updateResult.IsFailure) return updateResult;

        return Result.Success();
    }

    private static Result Validate(Guid counterpartyId)
    {
        if (counterpartyId == default)
        {
            return Result.Failure(Errors.PeerTransfer.CounterpartyRequired);
        }

        return Result.Success();
    }
}

