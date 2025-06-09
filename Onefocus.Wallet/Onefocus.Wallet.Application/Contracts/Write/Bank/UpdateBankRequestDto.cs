namespace Onefocus.Wallet.Application.Contracts.Write.Bank;

public sealed record UpdateBankRequestDto(Guid Id, string Name, string? Description, bool IsActive, Guid ActionedBy);