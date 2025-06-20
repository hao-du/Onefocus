using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.Transaction.BankAccount;

public sealed record GetBankAccountByIdResponseDto(List<Entity.BankAccount> Transactions);