using Onefocus.Common.Abstractions.Messages;
using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Domain.Messages.Read.Currency;

public sealed record GetAllCurrenciesResponseDto(List<CurrencyResponse> Currencies)
{
    public static GetAllCurrenciesResponseDto Cast(List<Entity.Currency> source)
    {
        var currencyResponses = new GetAllCurrenciesResponseDto(
            Currencies: source.Select(c => new CurrencyResponse(
                Id: c.Id,
                Name: c.Name,
                ShortName: c.ShortName,
                DefaultFlag: c.DefaultFlag,
                ActiveFlag: c.ActiveFlag,
                Description: c.Description,
                ActionedOn: c.UpdatedOn ?? c.CreatedOn,
                ActionedBy: c.UpdatedBy ?? c.CreatedBy
            )).ToList()
        );

        return currencyResponses;
    }
}
