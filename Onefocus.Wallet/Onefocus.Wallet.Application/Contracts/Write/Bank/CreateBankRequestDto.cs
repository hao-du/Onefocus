using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.Bank
{
    public sealed record CreateBankRequestDto(Entity.Bank Bank);
}