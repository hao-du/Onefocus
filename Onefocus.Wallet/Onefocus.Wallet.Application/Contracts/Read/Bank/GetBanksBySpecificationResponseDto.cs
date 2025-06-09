using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Bank;

public sealed record GetBanksBySpecificationResponseDto(List<Entity.Bank> Banks);