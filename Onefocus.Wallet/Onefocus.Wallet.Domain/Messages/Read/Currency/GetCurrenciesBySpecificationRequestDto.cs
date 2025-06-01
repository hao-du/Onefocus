using Entity = Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Common.Abstractions.Domain.Specification;

namespace Onefocus.Wallet.Domain.Messages.Read.Currency;

public sealed record GetCurrenciesBySpecificationRequestDto(Specification<Entity.Currency> Specification);