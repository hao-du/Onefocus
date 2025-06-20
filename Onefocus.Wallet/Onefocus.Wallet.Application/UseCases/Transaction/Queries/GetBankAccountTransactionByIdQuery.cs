using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Queries;
public sealed record GetCashFlowByIdQueryRequest(Guid Id) : IQuery<GetCashFlowByIdQueryResponse>;

public sealed record GetCashFlowByIdQueryResponse(Guid Id, DateTimeOffset TransactedOn, Guid CurrencyId, bool IsIncome, decimal Amount, string? Description, bool IsActive);

internal sealed class GetCashFlowByIdQueryHandler(
    IReadUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<GetCashFlowByIdQueryRequest, GetCashFlowByIdQueryResponse>(httpContextAccessor)
{
    public override async Task<Result<GetCashFlowByIdQueryResponse>> Handle(GetCashFlowByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);
        var userId = getUserIdResult.Value;

        var getCashFlowResult = await unitOfWork.Transaction.GetCashFlowByIdAsync(new(request.Id, userId), cancellationToken);
        if (getCashFlowResult.IsFailure) return Failure(getCashFlowResult);

        var transaction = getCashFlowResult.Value.Transaction;
        if (transaction == null) return Result.Success<GetCashFlowByIdQueryResponse>(null);

        var response = new GetCashFlowByIdQueryResponse(
            Id: transaction.Id,
            TransactedOn: transaction.TransactedOn,
            CurrencyId: transaction.CurrencyId,
            IsIncome: transaction.CashFlows.Single().IsIncome,
            Amount: transaction.Amount,
            Description: transaction.Description,
            IsActive: transaction.IsActive
        );
        return Result.Success(response);
    }
}
