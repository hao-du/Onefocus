using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Transaction;

public sealed record GetAllTransactionsResponseDto(IReadOnlyList<Entity.Transaction> Transactions);
