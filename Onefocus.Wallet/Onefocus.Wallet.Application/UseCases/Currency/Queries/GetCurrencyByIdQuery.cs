using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Wallet.Application.UseCases.Transaction.Queries;

namespace Onefocus.Wallet.Application.UseCases.Currency.Queries;

public sealed record GetCurrencyByIdQueryRequest(Guid Id) : IQuery<GetCurrencyByIdQueryResponse>;
public sealed record GetCurrencyByIdQueryResponse(Guid Id, string Name, string ShortName, bool IsDefault, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);

internal sealed class GetCurrencyByIdQueryHandler(
    IHttpContextAccessor httpContextAccessor,
    ILogger<GetAllTransactionsQueryHandler> logger,
    IReadUnitOfWork unitOfWork
) : QueryHandler<GetCurrencyByIdQueryRequest, GetCurrencyByIdQueryResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<GetCurrencyByIdQueryResponse>> Handle(GetCurrencyByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);
        var userId = getUserIdResult.Value;

        var currencyResult = await unitOfWork.Currency.GetCurrencyByIdAsync(new(request.Id, userId), cancellationToken);
        if (currencyResult.IsFailure)
        {
            return Result.Failure<GetCurrencyByIdQueryResponse>(currencyResult.Errors);
        }

        var currency = currencyResult.Value.Currency;
        if (currency == null) return Result.Success<GetCurrencyByIdQueryResponse>(null);

        return Result.Success(new GetCurrencyByIdQueryResponse(
            Id: currency.Id,
            Name: currency.Name,
            ShortName: currency.ShortName,
            IsDefault: currency.IsDefault,
            IsActive: currency.IsActive,
            Description: currency.Description,
            ActionedOn: currency.UpdatedOn ?? currency.CreatedOn,
            ActionedBy: currency.UpdatedBy ?? currency.CreatedBy
        ));
    }
}

