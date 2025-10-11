using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Wallet.Application.UseCases.Transaction.Queries;

namespace Onefocus.Wallet.Application.UseCases.Currency.Queries;

public sealed record GetAllCurrenciesQueryRequest() : IQuery<GetAllCurrenciesQueryResponse>;
public sealed record GetAllCurrenciesQueryResponse(List<CurrencyQueryResponse> Currencies);
public record CurrencyQueryResponse(Guid Id, string Name, string ShortName, bool IsDefault, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);

internal sealed class GetAllCurrenciesQueryHandler(
    IHttpContextAccessor httpContextAccessor,
    ILogger<GetAllTransactionsQueryHandler> logger,
    IReadUnitOfWork unitOfWork
) : QueryHandler<GetAllCurrenciesQueryRequest, GetAllCurrenciesQueryResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<GetAllCurrenciesQueryResponse>> Handle(GetAllCurrenciesQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);
        var userId = getUserIdResult.Value;

        var currencyDtosResult = await unitOfWork.Currency.GetAllCurrenciesAsync(new(userId), cancellationToken);
        if (currencyDtosResult.IsFailure) return currencyDtosResult.Failure<GetAllCurrenciesQueryResponse>();
        var currencyDtos = currencyDtosResult.Value.Currencies;
        return Result.Success(new GetAllCurrenciesQueryResponse(
            Currencies: [.. currencyDtos.Select(c => new CurrencyQueryResponse(
                Id: c.Id,
                Name: c.Name,
                ShortName: c.ShortName,
                IsDefault: c.IsDefault,
                IsActive: c.IsActive,
                Description: c.Description,
                ActionedOn: c.UpdatedOn ?? c.CreatedOn,
                ActionedBy: c.UpdatedBy ?? c.UpdatedBy
            ))]
        ));
    }
}
