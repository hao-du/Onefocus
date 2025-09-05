using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Write.Counterparty;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Write;

public interface ICounterpartyWriteRepository : IBaseContextRepository
{
    Task<Result<GetCounterpartyByIdResponseDto>> GetCounterpartyByIdAsync(GetCounterpartyByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddCounterpartyAsync(CreateCounterpartyRequestDto request, CancellationToken cancellationToken = default);
}