namespace Onefocus.Wallet.Application.Contracts.Read.Counterparty;

public sealed record GetCounterpartyByIdRequestDto(Guid Id, Guid UserId);