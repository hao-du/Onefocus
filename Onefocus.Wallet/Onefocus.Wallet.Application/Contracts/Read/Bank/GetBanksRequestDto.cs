namespace Onefocus.Wallet.Application.Contracts.Read.Bank;

public sealed record GetBanksRequestDto(Guid UserId, string? Name, string? Description);
