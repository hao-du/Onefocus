using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Queries;
public sealed record GetCashFlowByTransactionIdQueryRequest(Guid TransactionId) : IQuery<GetCashFlowByTransactionIdQueryResponse>;

public sealed record GetCashFlowByTransactionIdQueryResponse(Guid Id, Guid TransactionId, DateTimeOffset TransactedOn, Guid CurrencyId, bool IsIncome, decimal Amount, string? Description, bool IsActive, IReadOnlyList<GetTransactionItem> TransactionItems);
public sealed record GetTransactionItem(Guid? Id, string Name, decimal Amount, bool IsActive, string? Description);

internal sealed class GetCashFlowByTransactionIdQueryHandler(
    ILogger<GetCashFlowByTransactionIdQueryHandler> logger,
    IReadUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<GetCashFlowByTransactionIdQueryRequest, GetCashFlowByTransactionIdQueryResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<GetCashFlowByTransactionIdQueryResponse>> Handle(GetCashFlowByTransactionIdQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);
        var userId = getUserIdResult.Value;

        var getCashFlowResult = await unitOfWork.Transaction.GetCashFlowByTransactionIdAsync(new(request.TransactionId, userId), cancellationToken);
        if (getCashFlowResult.IsFailure) return Failure(getCashFlowResult);

        var cashFlow = getCashFlowResult.Value.CashFlow;
        if (cashFlow == null) return Result.Success<GetCashFlowByTransactionIdQueryResponse>(null);
        var transaction = cashFlow.Transaction;

        var response = new GetCashFlowByTransactionIdQueryResponse(
            Id: cashFlow.Id,
            TransactionId: transaction.Id,
            TransactedOn: transaction.TransactedOn,
            CurrencyId: transaction.CurrencyId,
            IsIncome: cashFlow.IsIncome,
            Amount: transaction.Amount,
            Description: transaction.Description,
            IsActive: transaction.IsActive,
            TransactionItems: [..transaction.TransactionItems.Select(t => new GetTransactionItem(t.Id, t.Name, t.Amount, t.IsActive, t.Description))]
        );
        return Result.Success(response);
    }
}
