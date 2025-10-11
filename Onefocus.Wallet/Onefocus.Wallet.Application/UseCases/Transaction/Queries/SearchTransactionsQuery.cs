using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Wallet.Application.UseCases.Transaction.Queries.Repsonses;
using Onefocus.Wallet.Domain.Entities.Enums;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Queries;
public sealed record SearchTransactionsQueryRequest(Guid TransactionId) : IQuery<SearchTransactionsQueryResponse>;

public sealed record SearchTransactionsQueryResponse(IReadOnlyList<TransactionQueryResponse> Transactions);

internal sealed class SearchTransactionsQueryHandler(
    ILogger<SearchTransactionsQueryHandler> logger,
    IReadUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<SearchTransactionsQueryRequest, SearchTransactionsQueryResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<SearchTransactionsQueryResponse>> Handle(SearchTransactionsQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);
        var userId = getUserIdResult.Value;


        return Result.Success(new SearchTransactionsQueryResponse([]));
    }
}
