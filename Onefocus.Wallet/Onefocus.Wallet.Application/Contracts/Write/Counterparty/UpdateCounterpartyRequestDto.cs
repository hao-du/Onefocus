namespace Onefocus.Wallet.Application.Contracts.Write.Counterparty;

public sealed record UpdateCounterpartyRequestDto(Guid Id, string Name, string? Description, bool IsActive, Guid ActionedBy);