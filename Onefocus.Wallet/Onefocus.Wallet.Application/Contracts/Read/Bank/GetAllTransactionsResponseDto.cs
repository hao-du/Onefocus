using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Bank;

public sealed record GetAllTransactionsResponseDto(IReadOnlyList<Entity.Transaction> Transactions);
