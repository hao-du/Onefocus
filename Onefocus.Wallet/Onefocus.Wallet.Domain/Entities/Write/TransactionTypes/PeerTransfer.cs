using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Onefocus.Wallet.Domain.Events.Transaction;
using System;
using static Onefocus.Wallet.Domain.Errors;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class PeerTransfer : WriteEntityBase, IAggregateRoot
{
    private readonly List<PeerTransferTransaction> _peerTransferTransactions = [];

    public Guid CounterpartyId { get; private set; }
    public PeerTransferStatus Status { get; private set; }
    public PeerTransferType Type { get; private set; }

    public Counterparty Counterparty { get; private set; } = default!;

    public IReadOnlyCollection<PeerTransferTransaction> PeerTransferTransactions => _peerTransferTransactions.AsReadOnly();

    private PeerTransfer()
    {
        // Required for EF Core
    }

    private PeerTransfer(PeerTransferStatus status, PeerTransferType type, Counterparty counterparty, string? description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Status = status;
        Type = type;
        Counterparty = counterparty;
        CounterpartyId = counterparty.Id;
    }

    public static Result<PeerTransfer> Create(int status, int type, Counterparty counterparty, string? description, Guid ownerId, Guid actionedBy, IReadOnlyList<TransferTransactionParams> transactionParams)
    {
        var validationResult = Validate(counterparty, status, type, transactionParams.ToArray());
        if (validationResult.IsFailure) return validationResult.Failure<PeerTransfer>();

        var peerTransfer = new PeerTransfer((PeerTransferStatus)status, (PeerTransferType)type, counterparty, description, actionedBy);

        if (transactionParams.Count > 0)
        {
            var upsertInterestsResult = peerTransfer.UpsertTransferDetails(ownerId, actionedBy, transactionParams);
            if (upsertInterestsResult.IsFailure) return upsertInterestsResult.Failure<PeerTransfer>();
        }

        peerTransfer.AddDomainEvent(PeerTransferUpsertedEvent.Create(peerTransfer));

        return Result.Success(peerTransfer);
    }

    public Result Update(int status, int type, Counterparty counterparty, string? description, bool isActive, Guid ownerId, Guid actionedBy, IReadOnlyList<TransferTransactionParams> transactionParams)
    {
        var validationResult = Validate(counterparty, status, type, transactionParams.ToArray());
        if (validationResult.IsFailure) return validationResult;

        Status = (PeerTransferStatus)status;
        Type = (PeerTransferType)type;
        Counterparty = counterparty;
        CounterpartyId = counterparty.Id;
        Description = description;

        SetActiveFlag(isActive, actionedBy);

        var upsertInterestsResult = UpsertTransferDetails(ownerId, actionedBy, transactionParams);
        if (upsertInterestsResult.IsFailure) return upsertInterestsResult;

        var deleteInterestsResult = DeleteTransferDetails(actionedBy, transactionParams);
        if (deleteInterestsResult.IsFailure) return deleteInterestsResult;

        UpsertTransferDetails(ownerId, actionedBy, transactionParams);

        AddDomainEvent(PeerTransferUpsertedEvent.Create(this));

        return Result.Success();
    }

    private Result UpsertTransferDetails(Guid ownerId, Guid actionedBy, IReadOnlyList<TransferTransactionParams> transactionParams)
    {
        foreach (var param in transactionParams)
        {
            var isNew = !param.Id.HasValue || param.Id == Guid.Empty;
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
            currency: param.Currency,
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

        var updateResult = peerTransferTransaction.Transaction.Update(param.Amount, param.TransactedOn, param.Currency, param.IsActive, param.Description, actionedBy);
        if (updateResult.IsFailure) return updateResult;

        return Result.Success();
    }

    private Result DeleteTransferDetails(Guid actionedBy, IReadOnlyList<TransactionParams> transactionParams)
    {
        var peerTransferTransactionsToBeDeleted = _peerTransferTransactions.FindAll(t => !transactionParams.Any(param => param.Id == t.TransactionId));

        if (peerTransferTransactionsToBeDeleted.Count == 0) return Result.Success();

        foreach (var peerTransferTransaction in peerTransferTransactionsToBeDeleted)
        {
            peerTransferTransaction.SetActiveFlag(false, actionedBy);
            peerTransferTransaction.Transaction.SetActiveFlag(false, actionedBy);
        }

        return Result.Success();
    }

    public static Result Validate(Counterparty? counterparty, int status, int type, Array transactions)
    {
        if (counterparty == null)
        {
            return Result.Failure(Errors.PeerTransfer.CounterpartyRequired);
        }
        if (!Enum.IsDefined(typeof(PeerTransferStatus), status))
        {
            return Result.Failure(Errors.PeerTransfer.InvalidStatus);
        }
        if (!Enum.IsDefined(typeof(PeerTransferType), type))
        {
            return Result.Failure(Errors.PeerTransfer.InvalidType);
        }
        if(transactions == null || transactions.Length == 0)
        {
            return Result.Failure(Errors.PeerTransfer.NoTransactionsProvided);
        }

        return Result.Success();
    }
}

