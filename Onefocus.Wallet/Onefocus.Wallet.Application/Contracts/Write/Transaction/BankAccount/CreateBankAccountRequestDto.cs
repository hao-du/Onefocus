using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.Transaction.BankAccount;

public sealed record CreateBankAccountRequestDto(Entity.TransactionTypes.BankAccount BankAccount);