using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.Counterparty;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Read;

public interface ICounterpartyReadRepository : IBaseContextRepository
{
    Task<Result<GetAllCounterpartiesResponseDto>> GetAllCounterpartysAsync(GetAllCounterpartiesRequestDto request, CancellationToken cancellationToken = default);

    Task<Result<GetCounterpartyByIdResponseDto>> GetCounterpartyByIdAsync(GetCounterpartyByIdRequestDto request, CancellationToken cancellationToken = default);

}