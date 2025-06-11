using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.Bank;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Read;

public interface ITransactionReadRepository : IBaseContextRepository
{
    Task<Result<GetAllTransactionsResponseDto>> GetAllTransactionsAsync(GetAllTransactionsRequestDto request, CancellationToken cancellationToken = default);

}