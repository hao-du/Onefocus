using Microsoft.AspNetCore.Http;
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
    IReadUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<GetCurrencyExchangeByTransactionIdQueryRequest, GetCurrencyExchangeByTransactionIdQueryResponse>(httpContextAccessor)
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

        var response = new GetCurrencyExchangeByTransactionIdQueryResponse(
            Id: currencyExchange.Id,
            Amount: currencyExchange.Amount,
            CurrencyId: currencyExchange.CurrencyId,
            InterestRate: currencyExchange.InterestRate,
            AccountNumber: currencyExchange.AccountNumber,
            IssuedOn: currencyExchange.IssuedOn,
            ClosedOn: currencyExchange.ClosedOn,
            IsClosed: currencyExchange.IsClosed,
            BankId: currencyExchange.BankId,
            IsActive: currencyExchange.IsActive,
            Description: currencyExchange.Description,
            Transactions: [..currencyExchange.CurrencyExchangeTransactions.Select(bct => new GetCurrencyExchangeTransaction(
                Id: bct.Transaction.Id,
                TransactedOn: bct.Transaction.TransactedOn,
                CurrencyId: bct.Transaction.CurrencyId,
                Amount: bct.Transaction.Amount,
                Description: bct.Transaction.Description,
                IsActive: bct.Transaction.IsActive
            ))]
        );
        return Result.Success(response);
    }
}
