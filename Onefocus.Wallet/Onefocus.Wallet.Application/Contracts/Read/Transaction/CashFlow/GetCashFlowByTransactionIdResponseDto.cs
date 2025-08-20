using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Transaction.CashFlow;

public sealed record GetCashFlowByTransactionIdResponseDto(Entity.TransactionTypes.CashFlow? CashFlow);
