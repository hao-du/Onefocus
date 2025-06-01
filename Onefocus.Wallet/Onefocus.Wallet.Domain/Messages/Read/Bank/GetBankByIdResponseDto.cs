using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Domain.Messages.Read.Bank;

public sealed record GetBankByIdResponseDto(Entity.Bank? Bank);
