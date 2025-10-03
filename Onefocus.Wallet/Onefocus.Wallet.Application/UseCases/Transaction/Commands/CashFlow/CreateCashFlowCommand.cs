using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Commands.CashFlow;
public sealed record CreateCashFlowCommandRequest(decimal Amount, DateTimeOffset TransactedOn, bool IsIncome, Guid CurrencyId, string? Description, IReadOnlyList<CreateTransactionItem> TransactionItems) : ICommand<CreateCashFlowCommandResponse>;
public sealed record CreateTransactionItem(string Name, decimal Amount, string? Description);
public sealed record CreateCashFlowCommandResponse(Guid Id);

internal sealed class CreateCashFlowCommandHandler(
    ILogger<CreateCashFlowCommandHandler> logger
        , IWriteUnitOfWork unitOfWork
        , IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<CreateCashFlowCommandRequest, CreateCashFlowCommandResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<CreateCashFlowCommandResponse>> Handle(CreateCashFlowCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return Failure(validationResult);

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return Failure(actionByResult);

        var addCashFlowResult = Entity.TransactionTypes.CashFlow.Create(
               amount: request.Amount,
               transactedOn: request.TransactedOn,
               isIncome: request.IsIncome,
               currencyId: request.CurrencyId,
               description: request.Description,
               ownerId: actionByResult.Value,
               actionedBy: actionByResult.Value,
               transactionItems: [.. request.TransactionItems.Select(item => TransactionItemParams.CreateNew(item.Name, item.Amount, item.Description))]
            );
        if (addCashFlowResult.IsFailure) return Failure(addCashFlowResult);

        var cashFlow = addCashFlowResult.Value;
        var repoResult = await unitOfWork.Transaction.AddCashFlowAsync(new(cashFlow), cancellationToken);
        if (repoResult.IsFailure) return Failure(repoResult);

        var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return Failure(saveChangesResult);

        return Result.Success<CreateCashFlowCommandResponse>(new(cashFlow.Id));
    }

    private static Result ValidateRequest(CreateCashFlowCommandRequest request)
    {
        var validationResult = Entity.Transaction.Validate(
            request.Amount,
            request.CurrencyId,
            request.TransactedOn
        );
        if(validationResult.IsFailure) return validationResult;

        foreach (var item in request.TransactionItems)
        {
            var itemValidationResult = Entity.TransactionItem.Validate(item.Name, item.Amount);
            if (itemValidationResult.IsFailure) return itemValidationResult;
        }

        return Result.Success();
    }
}

