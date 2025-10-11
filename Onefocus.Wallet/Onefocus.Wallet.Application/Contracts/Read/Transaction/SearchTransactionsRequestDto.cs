using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Transaction;

public record SearchTransactionsResponseDto(IReadOnlyList<Entity.Transaction> Transactions);
