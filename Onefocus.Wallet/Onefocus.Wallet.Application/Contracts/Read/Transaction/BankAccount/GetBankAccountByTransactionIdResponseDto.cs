using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Transaction.BankAccount;

public sealed record GetBankAccountByTransactionIdResponseDto(Entity.TransactionTypes.BankAccount? BankAccount);
