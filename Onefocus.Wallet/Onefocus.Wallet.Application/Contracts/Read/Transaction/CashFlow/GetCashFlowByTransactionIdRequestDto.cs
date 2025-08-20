namespace Onefocus.Wallet.Application.Contracts.Read.Transaction.CashFlow;

public sealed record GetCashFlowByTransactionIdRequestDto(Guid TransactionId, Guid UserId);