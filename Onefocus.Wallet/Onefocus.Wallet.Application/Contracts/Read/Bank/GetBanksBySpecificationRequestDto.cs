using Onefocus.Common.Abstractions.Domain.Specification;
using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Bank;

public sealed record GetBanksBySpecificationRequestDto(Specification<Entity.Bank> Specification);