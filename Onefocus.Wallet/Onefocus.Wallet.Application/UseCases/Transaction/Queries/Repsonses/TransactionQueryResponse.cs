using Onefocus.Wallet.Domain.Entities.Enums;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Queries.Repsonses;

public sealed record TransactionQueryResponse(Guid Id, DateTimeOffset TransactedOn, string CurrencyName, TransactionType Type, IReadOnlyList<string> Tags, decimal Amount, string? Description);
