namespace Onefocus.Wallet.Domain.Messages.Write;

public sealed record BulkMarkDefaultFlagRequestDto(IReadOnlyList<Guid> ExcludeIds, bool QueryValue, bool UpdatingValue, Guid ActionedBy);
