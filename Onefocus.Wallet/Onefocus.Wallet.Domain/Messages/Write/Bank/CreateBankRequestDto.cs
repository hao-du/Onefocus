using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Messages.Write.Bank;

public sealed record CreateBankRequestDto(Entity.Bank Bank);