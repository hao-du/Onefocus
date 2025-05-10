using Onefocus.Common.Abstractions.Messages;
using Entity = Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Common.Abstractions.Domain.Specification;

namespace Onefocus.Wallet.Domain.Messages.Read.Currency;

public sealed record GetCurrenciesBySpecificationResponseDto(List<CurrencyResponse> Currencies)
{
    public static GetCurrenciesBySpecificationResponseDto Cast(List<Entity.Currency> source)
    {
        var currencyDto = new GetCurrenciesBySpecificationResponseDto(
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

        return currencyDto;
    }
}