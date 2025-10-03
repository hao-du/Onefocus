using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Commands.PeerTransfer;
public sealed record CreatePeerTransferCommandRequest(
    int Status, 
    int Type, 
    Guid CounterpartyId, 
    string? Description, 
    IReadOnlyList<CreateTransferTransaction> TransferTransactions
) : ICommand<CreatePeerTransferCommandResponse>;
public sealed record CreateTransferTransaction(
    decimal Amount,
    DateTimeOffset TransactedOn,
    Guid CurrencyId,
    bool IsInFlow,
    string? Description
);
public sealed record CreatePeerTransferCommandResponse(Guid Id);

internal sealed class CreatePeerTransferCommandHandler(
        ILogger<CreatePeerTransferCommandHandler> logger
        , IWriteUnitOfWork unitOfWork
        , IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<CreatePeerTransferCommandRequest, CreatePeerTransferCommandResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<CreatePeerTransferCommandResponse>> Handle(CreatePeerTransferCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return Failure(validationResult);

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return Failure(actionByResult);

        var addPeerTransferResult = Entity.TransactionTypes.PeerTransfer.Create(
               status: request.Status,
               type: request.Type,
               counterpartyId: request.CounterpartyId,
               description: request.Description,
               ownerId: actionByResult.Value,
               actionedBy: actionByResult.Value,
               transactionParams: [.. request.TransferTransactions.Select(t => TransferTransactionParams.CreateNew(
                    amount: t.Amount,
                    transactedOn: t.TransactedOn,
                    currencyId: t.CurrencyId,
                    isInFlow: t.IsInFlow,
                    description: t.Description
                ))]
            );
        if (addPeerTransferResult.IsFailure) return Failure(addPeerTransferResult);

        var peerTransfer = addPeerTransferResult.Value;
        var repoResult = await unitOfWork.Transaction.AddPeerTransferAsync(new(peerTransfer), cancellationToken);
        if (repoResult.IsFailure) return Failure(repoResult);

        var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return Failure(saveChangesResult);

        return Result.Success<CreatePeerTransferCommandResponse>(new(peerTransfer.Id));
    }

    private static Result ValidateRequest(CreatePeerTransferCommandRequest request)
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

