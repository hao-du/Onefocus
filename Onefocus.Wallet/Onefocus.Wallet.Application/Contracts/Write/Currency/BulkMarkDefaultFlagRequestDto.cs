namespace Onefocus.Wallet.Application.Contracts.Write.Currency;

public sealed record BulkMarkDefaultFlagRequestDto(IReadOnlyList<Guid> ExcludeIds, bool QueryValue, bool UpdatingValue, Guid ActionedBy);
