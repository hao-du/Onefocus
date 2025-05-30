namespace Onefocus.Wallet.Domain.Messages.Write.Currency;

public sealed record BulkMarkDefaultFlagRequestDto(IReadOnlyList<Guid> ExcludeIds, bool QueryValue, bool UpdatingValue, Guid ActionedBy);
