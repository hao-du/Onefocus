using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
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
    IDomainEventService domainEventService,
    ILogger<UpdatePeerTransferCommandHandler> logger,
    IWriteUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<UpdatePeerTransferCommandRequest>(httpContextAccessor, logger)
{
    public override async Task<Result> Handle(UpdatePeerTransferCommandRequest request, CancellationToken cancellationToken)
    {
        var getCurrenciesResult = await unitOfWork.Currency.GetCurrenciesByIdsAsync(new([.. request.TransferTransactions.Select(t => t.CurrencyId)]), cancellationToken);
        if (getCurrenciesResult.IsFailure) { return getCurrenciesResult; }
        var currencies = getCurrenciesResult.Value.Currencies;

        var getCounterpartyResult = await unitOfWork.Counterparty.GetCounterpartyByIdAsync(new(request.CounterpartyId), cancellationToken);
        if (getCounterpartyResult.IsFailure) { return getCounterpartyResult; }
        var counterparty = getCounterpartyResult.Value.Counterparty;

        var validationResult = ValidateRequest(request, counterparty, currencies);
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
            counterparty: counterparty!,
            description: request.Description,
            isActive: request.IsActive,
            ownerId: actionByResult.Value,
            actionedBy: actionByResult.Value,
            transactionParams: [..request.TransferTransactions.Select(t => TransferTransactionParams.Create(
                id: t.Id,
                amount: t.Amount,
                transactedOn: t.TransactedOn,
                currency: currencies.First(c => c.Id == t.CurrencyId),
                isInFlow: t.IsInFlow,
                isActive: t.IsActive,
                description: t.Description
            ))]
        );
        if (updateCashflowResult.IsFailure) return updateCashflowResult;

        if (peerTransfer.DomainEvents.Count > 0)
        {
            var addSearchIndexEventResult = await domainEventService.AddSearchIndexEvent(peerTransfer.DomainEvents, cancellationToken);
            if (addSearchIndexEventResult.IsFailure) return addSearchIndexEventResult;
            peerTransfer.ClearDomainEvents();
        }

        var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return saveChangesResult;
        return Result.Success();
    }

    private static Result ValidateRequest(UpdatePeerTransferCommandRequest request, Entity.Counterparty? counterparty, IReadOnlyList<Entity.Currency> currencies)
    {
        var validationResult = Entity.TransactionTypes.PeerTransfer.Validate(
            counterparty,
            request.Status,
            request.Type,
            request.TransferTransactions.ToArray()
        );
        if (validationResult.IsFailure) return validationResult;

        foreach (var item in request.TransferTransactions)
        {
            var itemValidationResult = Entity.Transaction.Validate(
                item.Amount,
                currencies.FirstOrDefault(c => c.Id == item.CurrencyId),
                item.TransactedOn
            );
            if (itemValidationResult.IsFailure) return itemValidationResult;
        }

        return Result.Success();
    }
}

