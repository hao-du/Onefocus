using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.Currency.Queries;

public sealed record GetAllCurrenciesQueryRequest() : IQuery<GetAllCurrenciesQueryResponse>;
public sealed record GetAllCurrenciesQueryResponse(List<CurrencyQueryResponse> Currencies);
public record CurrencyQueryResponse(Guid Id, string Name, string ShortName, bool IsDefault, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);

internal sealed class GetAllCurrenciesQueryHandler(IReadUnitOfWork unitOfWork) : IQueryHandler<GetAllCurrenciesQueryRequest, GetAllCurrenciesQueryResponse>
{
    public async Task<Result<GetAllCurrenciesQueryResponse>> Handle(GetAllCurrenciesQueryRequest request, CancellationToken cancellationToken)
    {
        var currencyDtosResult = await unitOfWork.Currency.GetAllCurrenciesAsync(cancellationToken);
        if (currencyDtosResult.IsFailure) return Result.Failure<GetAllCurrenciesQueryResponse>(currencyDtosResult.Errors);
        var currencyDtos = currencyDtosResult.Value.Currencies;
        return Result.Success<GetAllCurrenciesQueryResponse>(new GetAllCurrenciesQueryResponse(
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

