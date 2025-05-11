using MediatR;
using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Wallet.Domain.Messages.Read.Currency;
using Onefocus.Wallet.Domain.Repositories.Read;
using static Onefocus.Membership.Application.User.Commands.GetAllCurrenciesQueryResponse;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record GetAllCurrenciesQueryRequest() : IQuery<GetAllCurrenciesQueryResponse>;

public sealed record GetAllCurrenciesQueryResponse(List<CurrencyQueryResponse> Currencies)
{
    public static GetAllCurrenciesQueryResponse Cast(GetAllCurrenciesResponseDto source)
    {
        var currencyResponses = new GetAllCurrenciesQueryResponse(
            Currencies: source.Currencies.Select(c => new CurrencyQueryResponse(
                Id: c.Id,
                Name: c.Name,
                ShortName: c.ShortName,
                DefaultFlag: c.DefaultFlag,
                ActiveFlag: c.ActiveFlag,
                Description: c.Description,
                ActionedOn: c.UpdatedOn ?? c.CreatedOn,
                ActionedBy: c.UpdatedBy ?? c.UpdatedBy
            )).ToList()
        );

        return currencyResponses;
    }

    public record CurrencyQueryResponse(Guid Id, string Name, string ShortName, bool DefaultFlag, bool ActiveFlag, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);
}


internal sealed class GetAllCurrenciesQueryHandler : IQueryHandler<GetAllCurrenciesQueryRequest, GetAllCurrenciesQueryResponse>
{
    private readonly ICurrencyReadRepository _currencyRepository;
    

    public GetAllCurrenciesQueryHandler(ICurrencyReadRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<Result<GetAllCurrenciesQueryResponse>> Handle(GetAllCurrenciesQueryRequest request, CancellationToken cancellationToken)
    {
        var currencyDtosResult = await _currencyRepository.GetAllCurrenciesAsync(cancellationToken);
        if (currencyDtosResult == null)
        {
            return Result.Failure<GetAllCurrenciesQueryResponse>(CommonErrors.NullReference);
        } 
        if (currencyDtosResult.IsFailure)
        {
            return Result.Failure<GetAllCurrenciesQueryResponse>(currencyDtosResult.Error);
        }

        return Result.Success<GetAllCurrenciesQueryResponse>(GetAllCurrenciesQueryResponse.Cast(currencyDtosResult.Value));
    }
}

