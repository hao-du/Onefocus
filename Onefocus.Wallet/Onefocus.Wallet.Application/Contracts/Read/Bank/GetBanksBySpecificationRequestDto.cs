using Entity = Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Common.Abstractions.Domain.Specification;

namespace Onefocus.Wallet.Domain.Messages.Read.Bank;

public sealed record GetBanksBySpecificationRequestDto(Specification<Entity.Bank> Specification);