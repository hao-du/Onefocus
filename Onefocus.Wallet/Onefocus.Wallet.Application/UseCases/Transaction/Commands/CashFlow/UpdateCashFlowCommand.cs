using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Application.Services;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Commands.CashFlow;
public sealed record UpdateCashFlowCommandRequest(Guid Id, decimal Amount, DateTimeOffset TransactedOn, bool IsIncome, Guid CurrencyId, bool IsActive, string? Description, IReadOnlyList<UpdateTransactionItem> TransactionItems) : ICommand;
public sealed record UpdateTransactionItem(Guid? Id, string Name, decimal Amount, bool IsActive, string? Description);

internal sealed class UpdateCashFlowCommandHandler(
    ITransactionService transactionService,
    ILogger<UpdateCashFlowCommandHandler> logger,
    IWriteUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<UpdateCashFlowCommandRequest>(httpContextAccessor, logger)
{
    public override async Task<Result> Handle(UpdateCashFlowCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return validationResult;

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult;

        var getCashFlowResult = await unitOfWork.Transaction.GetCashFlowByIdAsync(new(request.Id), cancellationToken);
        if (getCashFlowResult.IsFailure) return getCashFlowResult;

        var cashFlow = getCashFlowResult.Value.CashFlow;
        if (cashFlow == null) return Result.Failure(CommonErrors.NullReference);

        var updateCashflowResult = cashFlow.Update(
            amount: request.Amount,
            transactedOn: request.TransactedOn,
            isIncome: request.IsIncome,
            currencyId: request.CurrencyId,
            description: request.Description,
            isActive: request.IsActive,
            actionedBy: actionByResult.Value,
            transactionItems: [.. request.TransactionItems.Select(item => TransactionItemParams.Create(item.Id, item.Name, item.Amount, item.IsActive, item.Description))]
        );
        if (updateCashflowResult.IsFailure) return updateCashflowResult;

        var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return saveChangesResult;

        await transactionService.PublishEvents(cashFlow, cancellationToken);

        return Result.Success();
    }

    private static Result ValidateRequest(UpdateCashFlowCommandRequest request)
    {
        var validationResult = Entity.Transaction.Validate(
            request.Amount,
            request.CurrencyId,
            request.TransactedOn
        );
        if (validationResult.IsFailure) return validationResult;

        foreach (var item in request.TransactionItems)
        {
            var itemValidationResult = Entity.TransactionItem.Validate(item.Name, item.Amount);
            if (itemValidationResult.IsFailure) return itemValidationResult;
        }

        return Result.Success();
    }
}

