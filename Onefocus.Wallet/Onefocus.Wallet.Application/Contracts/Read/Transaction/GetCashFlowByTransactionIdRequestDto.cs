namespace Onefocus.Wallet.Application.Contracts.Read.Transaction;

public sealed record GetCashFlowByTransactionIdRequestDto(Guid TransactionId, Guid UserId);