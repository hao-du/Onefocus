namespace Onefocus.Wallet.Application.Contracts.Read.Transaction;

public sealed record GetCashFlowByIdRequestDto(Guid Id, Guid UserId);
