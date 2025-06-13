using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Write.Params;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class PeerTransfer : BaseTransaction, IAggregateRoot
{
    public Guid TransferredUserId { get; private set; }
    public PeerTransferStatus Status { get; private set; }
    public PeerTransferType Type { get; private set; }

    public User TransferredUser { get; private set; } = default!;

    private PeerTransfer() : base()
    {
    }

    public PeerTransfer(Guid transferredUserId, PeerTransferStatus status, PeerTransferType type, string? description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        TransferredUserId = transferredUserId;
        Status = status;
        Type = type;
    }

    public static Result<PeerTransfer> Create(Guid transferredUserId, PeerTransferStatus status, PeerTransferType type, string? description, Guid actionedBy)
    {
        var validationResult = Validate(transferredUserId);
        if (validationResult.IsFailure) return (Result<PeerTransfer>)validationResult;

        return new PeerTransfer(transferredUserId, status, type, description, actionedBy);
    }

    public Result CreateTransferTransaction(decimal amount, DateTimeOffset transactedOn, Guid currencyId, string? description, Guid actionedBy)
    {
        return CreateTransaction(amount, transactedOn, currencyId, description, actionedBy);
    }

    public Result Update(Guid transferredUserId, PeerTransferStatus status, PeerTransferType type, bool isActive, string? description, Guid actionedBy, IReadOnlyList<TransactionParams> transactions)
    {
        var validationResult = Validate(transferredUserId);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        TransferredUserId = transferredUserId;
        Status = status;
        Type = type;
        Description = description;

        if (isActive) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        foreach (var transaction in transactions)
        {
            UpsertTransaction(transaction, actionedBy);
        }

        return Result.Success();
    }

    public Result Complete(Guid actionedBy)
    {
        return ChangeStatus(PeerTransferStatus.Completed, actionedBy);
    }

    public Result MarkAsFailed(Guid actionedBy)
    {
        return ChangeStatus(PeerTransferStatus.Failed, actionedBy);
    }

    public Result MarkAsProcessing(Guid actionedBy)
    {
        return ChangeStatus(PeerTransferStatus.Processing, actionedBy);
    }

    public Result MarkAsPending(Guid actionedBy)
    {
        return ChangeStatus(PeerTransferStatus.Pending, actionedBy);
    }

    private Result ChangeStatus(PeerTransferStatus status, Guid actionedBy)
    {
        var validationResult = Validate(TransferredUserId);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Status = status;
        Update(actionedBy);

        return Result.Success();
    }

    private static Result Validate(Guid transferredUserId)
    {
        if (transferredUserId == default)
        {
            return Result.Failure(Errors.User.UserRequired);
        }

        return Result.Success();
    }
}

