using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Read.Currency;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Read;
using static Onefocus.Wallet.Application.Currency.Queries.GetAllCurrenciesQueryResponse;

namespace Onefocus.Wallet.Application.Currency.Queries;

public sealed record GetAllCurrenciesQueryRequest() : IQuery<GetAllCurrenciesQueryResponse>;

public sealed record GetAllCurrenciesQueryResponse(List<CurrencyQueryResponse> Currencies)
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0305:Simplify collection initialization", Justification = "ToList() will be more readable.")]
    public static GetAllCurrenciesQueryResponse Cast(GetAllCurrenciesResponseDto source)
    {
        var currencyResponses = new GetAllCurrenciesQueryResponse(
            Currencies: source.Currencies.Select(c => new CurrencyQueryResponse(
                Id: c.Id,
                Name: c.Name,
                ShortName: c.ShortName,
                IsDefault: c.IsDefault,
                IsActive: c.IsActive,
                Description: c.Description,
                ActionedOn: c.UpdatedOn ?? c.CreatedOn,
                ActionedBy: c.UpdatedBy ?? c.UpdatedBy
            )).ToList()
        );

        return currencyResponses;
    }

    public record CurrencyQueryResponse(Guid Id, string Name, string ShortName, bool IsDefault, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);
}


internal sealed class GetAllCurrenciesQueryHandler(IReadUnitOfWork unitOfWork) : IQueryHandler<GetAllCurrenciesQueryRequest, GetAllCurrenciesQueryResponse>
{
    public async Task<Result<GetAllCurrenciesQueryResponse>> Handle(GetAllCurrenciesQueryRequest request, CancellationToken cancellationToken)
    {
        var currencyDtosResult = await unitOfWork.Currency.GetAllCurrenciesAsync(cancellationToken);
        if (currencyDtosResult.IsFailure)
        {
            return Result.Failure<GetAllCurrenciesQueryResponse>(currencyDtosResult.Errors);
        }

        return Result.Success(Cast(currencyDtosResult.Value));
    }
}

