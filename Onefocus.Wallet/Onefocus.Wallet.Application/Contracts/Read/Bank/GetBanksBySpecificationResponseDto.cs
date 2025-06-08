using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Domain.Messages.Read.Bank;

public sealed record GetBanksBySpecificationResponseDto(List<Entity.Bank> Banks);