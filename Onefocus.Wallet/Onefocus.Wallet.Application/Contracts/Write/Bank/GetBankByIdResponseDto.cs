using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Messages.Write.Bank;

public sealed record GetBankByIdResponseDto(Entity.Bank? Bank);