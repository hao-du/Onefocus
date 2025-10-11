using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Bank;

public sealed record GetAllBanksRequestDto(Guid UserId);
