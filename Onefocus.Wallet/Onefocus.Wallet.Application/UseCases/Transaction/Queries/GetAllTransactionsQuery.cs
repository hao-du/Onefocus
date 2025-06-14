using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Wallet.Domain.Entities.Enums;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Queries;

public sealed record GetAllTransactionsQueryRequest() : IQuery<GetAllTransactionsQueryResponse>;

public sealed record GetAllTransactionsQueryResponse(IReadOnlyList<TransactionQueryResponse> Transactions);
public sealed record TransactionQueryResponse(Guid Id, DateTimeOffset TransactedOn, Guid CurrencyId, TransactionType Type, IReadOnlyList<string> Tags, decimal Amount, string? Description);

internal sealed class GetAllTransactionsQueryHandler(
    IReadUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<GetAllTransactionsQueryRequest, GetAllTransactionsQueryResponse>(httpContextAccessor)
{
    public override async Task<Result<GetAllTransactionsQueryResponse>> Handle(GetAllTransactionsQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);

        var userId = getUserIdResult.Value;

        var getTransactionsResult = await unitOfWork.Transaction.GetAllTransactionsAsync(new(userId), cancellationToken);
        if (getTransactionsResult.IsFailure) return Failure(getTransactionsResult);

        var transactions = getTransactionsResult.Value.Transactions.Select(t => new TransactionQueryResponse(
            Id: t.Id,
            TransactedOn: t.TransactedOn,
            CurrencyId: t.CurrencyId,
            Type: t.GetTransactionType(),
            Tags: t.GetTransactionTags(),
            Amount: t.Amount,
            Description: t.Description
        )).ToList();

        return Result.Success<GetAllTransactionsQueryResponse>(new(transactions));
    }
}
