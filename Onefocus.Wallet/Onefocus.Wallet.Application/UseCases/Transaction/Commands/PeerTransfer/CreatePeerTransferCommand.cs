using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Application.UseCases.Transaction.Commands.CurrencyExchange;
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
    ITransactionService transactionService,
    ILogger<CreatePeerTransferCommandHandler> logger,
    IWriteUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<CreatePeerTransferCommandRequest, CreatePeerTransferCommandResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<CreatePeerTransferCommandResponse>> Handle(CreatePeerTransferCommandRequest request, CancellationToken cancellationToken)
    {
        var getCurrenciesResult = await unitOfWork.Currency.GetCurrenciesByIdsAsync(new([.. request.TransferTransactions.Select(t => t.CurrencyId)]), cancellationToken);
        if (getCurrenciesResult.IsFailure) { return Failure(getCurrenciesResult); }
        var currencies = getCurrenciesResult.Value.Currencies;

        var getCounterpartyResult = await unitOfWork.Counterparty.GetCounterpartyByIdAsync(new(request.CounterpartyId), cancellationToken);
        if (getCounterpartyResult.IsFailure) { return Failure(getCounterpartyResult); }
        var counterparty = getCounterpartyResult.Value.Counterparty;

        var validationResult = ValidateRequest(request, counterparty, currencies);
        if (validationResult.IsFailure) return Failure(validationResult);

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return Failure(actionByResult);

        var addPeerTransferResult = Entity.TransactionTypes.PeerTransfer.Create(
               status: request.Status,
               type: request.Type,
               counterparty: counterparty!,
               description: request.Description,
               ownerId: actionByResult.Value,
               actionedBy: actionByResult.Value,
               transactionParams: [.. request.TransferTransactions.Select(t => TransferTransactionParams.CreateNew(
                    amount: t.Amount,
                    transactedOn: t.TransactedOn,
                    currency: currencies.First(c => c.Id == t.CurrencyId),
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

        await transactionService.PublishEvents(peerTransfer, cancellationToken);

        return Result.Success<CreatePeerTransferCommandResponse>(new(peerTransfer.Id));
    }

    private static Result ValidateRequest(CreatePeerTransferCommandRequest request, Entity.Counterparty? counterparty, IReadOnlyList<Entity.Currency> currencies)
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

