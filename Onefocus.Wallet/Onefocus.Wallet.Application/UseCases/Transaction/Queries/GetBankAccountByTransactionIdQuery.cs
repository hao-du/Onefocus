using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Queries;
public sealed record GetBankAccountByTransactionIdQueryRequest(Guid TransactionId) : IQuery<GetBankAccountByTransactionIdQueryResponse>;

public sealed record GetBankAccountByTransactionIdQueryResponse(Guid Id, Guid TransactionId, DateTimeOffset TransactedOn, Guid CurrencyId, bool IsIncome, decimal Amount, string? Description, bool IsActive, IReadOnlyList<GetTransactionItem> TransactionItems);

internal sealed class GetBankAccountByTransactionIdQueryHandler(
    IReadUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<GetBankAccountByTransactionIdQueryRequest, GetBankAccountByTransactionIdQueryResponse>(httpContextAccessor)
{
    public override async Task<Result<GetBankAccountByTransactionIdQueryResponse>> Handle(GetBankAccountByTransactionIdQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);
        var userId = getUserIdResult.Value;

        var getCashFlowResult = await unitOfWork.Transaction.GetCashFlowByTransactionIdAsync(new(request.TransactionId, userId), cancellationToken);
        if (getCashFlowResult.IsFailure) return Failure(getCashFlowResult);

        var transaction = getCashFlowResult.Value.Transaction;
        if (transaction == null) return Result.Success<GetBankAccountByTransactionIdQueryResponse>(null);

        var cashFlow = transaction.CashFlows.Single();

        var response = new GetBankAccountByTransactionIdQueryResponse(
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
