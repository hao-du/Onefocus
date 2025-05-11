using Onefocus.Common.Abstractions.Messages;
using Entity = Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Common.Abstractions.Domain.Specification;

namespace Onefocus.Wallet.Domain.Messages.Read.Currency;

public sealed record GetCurrenciesBySpecificationResponseDto(List<Entity.Currency> Currencies);