using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Bank;

public sealed record GetAllBanksResponseDto(List<Entity.Bank> Banks);
