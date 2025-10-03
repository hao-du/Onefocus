using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Queries;
public sealed record GetCurrencyExchangeByTransactionIdQueryRequest(Guid TransactionId) : IQuery<GetCurrencyExchangeByTransactionIdQueryResponse>;

public sealed record GetCurrencyExchangeByTransactionIdQueryResponse(
    Guid Id,
    decimal SourceAmount,
    Guid SourceCurrencyId,
    decimal TargetAmount,
    Guid TargetCurrencyId,
    decimal ExchangeRate,
    DateTimeOffset TransactedOn,
    bool IsActive,
    string? Description
);

internal sealed class GetCurrencyExchangeByTransactionIdQueryHandler(
    ILogger<GetCurrencyExchangeByTransactionIdQueryHandler> logger,
    IReadUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<GetCurrencyExchangeByTransactionIdQueryRequest, GetCurrencyExchangeByTransactionIdQueryResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<GetCurrencyExchangeByTransactionIdQueryResponse>> Handle(GetCurrencyExchangeByTransactionIdQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);
        var userId = getUserIdResult.Value;

        var getCurrencyExchangeResult = await unitOfWork.Transaction.GetCurrencyExchangeByTransactionIdAsync(new(request.TransactionId, userId), cancellationToken);
        if (getCurrencyExchangeResult.IsFailure) return Failure(getCurrencyExchangeResult);

        var currencyExchange = getCurrencyExchangeResult.Value.CurrencyExchange;
        if (currencyExchange == null) return Result.Success<GetCurrencyExchangeByTransactionIdQueryResponse>(null);

        var source = currencyExchange.GetSource();
        var target = currencyExchange.GetTarget();

        var response = new GetCurrencyExchangeByTransactionIdQueryResponse(
            Id: currencyExchange.Id,
            SourceAmount: source.Amount,
            SourceCurrencyId: source.CurrencyId,
            TargetAmount: target.Amount,
            TargetCurrencyId: target.CurrencyId,
            ExchangeRate: currencyExchange.ExchangeRate,
            TransactedOn: currencyExchange.GetTransactedOn(),
            IsActive: currencyExchange.IsActive,
            Description: currencyExchange.Description
        );
        return Result.Success(response);
    }
}
