using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Write.Models;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public sealed class PeerTransfer : WriteEntityBase
{
    private List<Transaction> _transactions = new List<Transaction>();

    public decimal Amount { get; private set; }
    public Guid CurrencyId { get; private set; }
    public Guid TransferredUserId { get; private set; }
    public PeerTransferStatus Status { get; private set; }
    public PeerTransferType Type { get; private set; }

    public User TransferredUser { get; private set; } = default!;
    public Currency Currency { get; private set; } = default!;
    public IReadOnlyCollection<Transaction> TransactionDetails => _transactions.AsReadOnly();

    private PeerTransfer() : base()
    {
    }

    public PeerTransfer(decimal amount, Guid currencyId, Guid transferredUserId, PeerTransferStatus status, PeerTransferType type, string? description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Amount = amount;
        CurrencyId = currencyId;
        TransferredUserId = transferredUserId;
        Status = status;
        Type = type;
    }

    public static Result<PeerTransfer> Create(decimal amount, Guid currencyId, Guid transferredUserId, PeerTransferStatus status, PeerTransferType type, string? description, Guid actionedBy)
    {
        var validationResult = Validate(amount, currencyId, transferredUserId);
        if (validationResult.IsFailure)
        {
            return Result.Failure<PeerTransfer>(validationResult.Error);
        }

        return new PeerTransfer(amount, currencyId, transferredUserId, status, type, description, actionedBy);
    }

    public Result Update(decimal amount, Guid currencyId, Guid transferredUserId, PeerTransferStatus status, PeerTransferType type, bool isActive, string? description, Guid actionedBy)
    {
        var validationResult = Validate(amount, currencyId, transferredUserId);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Amount = amount;
        CurrencyId = currencyId;
        TransferredUserId = transferredUserId;
        Status = status;
        Type = type;
        Description = description;

        if (isActive) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

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
        var validationResult = Validate(Amount, CurrencyId, TransferredUserId);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Status = status;
        Update(actionedBy);

        return Result.Success();
    }

    private static Result Validate(decimal amount, Guid currencyId, Guid transferredUserId)
    {
        if (amount < 0)
        {
            return Result.Failure(Errors.Transaction.AmountMustEqualOrGreaterThanZero);
        }
        if (currencyId == default)
        {
            return Result.Failure(Errors.Currency.CurrencyRequired);
        }
        if (transferredUserId == default)
        {
            return Result.Failure(Errors.User.UserRequired);
        }

        return Result.Success();
    }
}

