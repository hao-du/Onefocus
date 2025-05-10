using Onefocus.Common.Abstractions.Messages;
using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Domain.Messages.Read.Currency;

public sealed record GetCurrencyByIdResponseDto(Guid Id, string Name, string ShortName, bool DefaultFlag, bool ActiveFlag, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy)
{
    public static GetCurrencyByIdResponseDto? Cast(Entity.Currency? source)
    {
        if (source == null) return null;

        var currencyDto = new GetCurrencyByIdResponseDto(
            Id: source.Id,
            Name: source.Name,
            ShortName: source.ShortName,
            DefaultFlag: source.DefaultFlag,
            ActiveFlag: source.ActiveFlag,
            Description: source.Description,
            ActionedOn: source.UpdatedOn ?? source.CreatedOn,
            ActionedBy: source.UpdatedBy ?? source.CreatedBy
        );

        return currencyDto;
    }
}