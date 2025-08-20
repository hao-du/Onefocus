namespace Onefocus.Wallet.Application.Contracts.Read.Transaction.BankAccount;

public sealed record GetBankAccountByTransactionIdRequestDto(Guid TransactionId, Guid UserId);