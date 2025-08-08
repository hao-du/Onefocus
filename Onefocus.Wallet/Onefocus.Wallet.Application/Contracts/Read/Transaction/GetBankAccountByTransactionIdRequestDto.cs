namespace Onefocus.Wallet.Application.Contracts.Read.Transaction;

public sealed record GetBankAccountByTransactionIdRequestDto(Guid TransactionId, Guid UserId);