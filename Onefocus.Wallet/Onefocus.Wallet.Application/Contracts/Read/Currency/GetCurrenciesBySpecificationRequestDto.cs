using Onefocus.Common.Abstractions.Domain.Specification;
using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Currency;

public sealed record GetCurrenciesBySpecificationRequestDto(Specification<Entity.Currency> Specification);