using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Entities.Write.Params;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Commands;
public sealed record UpdateCashFlowCommandRequest(Guid Id, decimal Amount, DateTimeOffset TransactedOn, bool IsIncome, Guid CurrencyId, bool IsActive, string? Description, IReadOnlyList<UpdateTransactionItem> TransactionItems) : ICommand;
public sealed record UpdateTransactionItem(Guid? Id, string Name, decimal Amount, bool IsActive, string? Description);

internal sealed class UpdateCashFlowCommandHandler(
        IWriteUnitOfWork unitOfWork
        , IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<UpdateCashFlowCommandRequest>(httpContextAccessor)
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

        return Result.Success<CreateCashFlowCommandResponse>(new(cashFlow.TransactionId));
    }

    private static Result ValidateRequest(UpdateCashFlowCommandRequest request)
    {
        if (request.Amount < 0)
        {
            return Result.Failure(Errors.Transaction.AmountMustEqualOrGreaterThanZero);
        }
        if (request.CurrencyId == default)
        {
            return Result.Failure(Errors.Currency.CurrencyRequired);
        }
        if (request.TransactedOn == default)
        {
            return Result.Failure(Errors.Transaction.TransactedOnRequired);
        }
        if (request.TransactionItems.Any(item => string.IsNullOrEmpty(item.Name) || item.Amount < 0))
        {
            return Result.Failure(Errors.TransactionItem.InvalidTransactionItem);
        }

        return Result.Success();
    }
}

