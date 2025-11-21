using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Commands.PeerTransfer;
public sealed record UpdatePeerTransferCommandRequest(
    Guid Id, 
    int Status, 
    int Type, 
    Guid CounterpartyId, 
    bool IsActive, 
    string? Description, 
    IReadOnlyList<UpdateTransferTransaction> TransferTransactions
) : ICommand;
public sealed record UpdateTransferTransaction(
    Guid? Id, 
    decimal Amount, 
    DateTimeOffset TransactedOn, 
    Guid CurrencyId, 
    bool IsInFlow, 
    bool IsActive, 
    string? Description
);

internal sealed class UpdatePeerTransferCommandHandler(
    ITransactionService transactionService,
    ILogger<UpdatePeerTransferCommandHandler> logger,
    IWriteUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<UpdatePeerTransferCommandRequest>(httpContextAccessor, logger)
{
    public override async Task<Result> Handle(UpdatePeerTransferCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return validationResult;

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult;

        var getPeerTransferResult = await unitOfWork.Transaction.GetPeerTransferByIdAsync(new(request.Id), cancellationToken);
        if (getPeerTransferResult.IsFailure) return getPeerTransferResult;

        var peerTransfer = getPeerTransferResult.Value.PeerTransfer;
        if (peerTransfer == null) return Result.Failure(CommonErrors.NullReference);

        var updateCashflowResult = peerTransfer.Update(
            status: request.Status,
            type: request.Type,
            counterpartyId: request.CounterpartyId,
            description: request.Description,
            isActive: request.IsActive,
            ownerId: actionByResult.Value,
            actionedBy: actionByResult.Value,
            transactionParams: [..request.TransferTransactions.Select(t => TransferTransactionParams.Create(
                id: t.Id,
                amount: t.Amount,
                transactedOn: t.TransactedOn,
                currencyId: t.CurrencyId,
                isInFlow: t.IsInFlow,
                isActive: t.IsActive,
                description: t.Description
            ))]
        );
        if (updateCashflowResult.IsFailure) return updateCashflowResult;

        var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return saveChangesResult;

        await transactionService.PublishEvents(peerTransfer, cancellationToken);

        return Result.Success();
    }

    private static Result ValidateRequest(UpdatePeerTransferCommandRequest request)
    {
        var validationResult = Entity.TransactionTypes.PeerTransfer.Validate(
            request.CounterpartyId,
            request.Status,
            request.Type,
            request.TransferTransactions.ToArray()
        );
        if (validationResult.IsFailure) return validationResult;

        foreach (var item in request.TransferTransactions)
        {
            var itemValidationResult = Entity.Transaction.Validate(
                item.Amount,
                item.CurrencyId,
                item.TransactedOn
            );
            if (itemValidationResult.IsFailure) return itemValidationResult;
        }

        return Result.Success();
    }
}

