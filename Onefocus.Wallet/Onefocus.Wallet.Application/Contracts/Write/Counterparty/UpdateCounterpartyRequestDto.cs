namespace Onefocus.Wallet.Application.Contracts.Write.Bank;

public sealed record UpdateCounterpartyRequestDto(Guid Id, string Name, string? Description, bool IsActive, Guid ActionedBy);