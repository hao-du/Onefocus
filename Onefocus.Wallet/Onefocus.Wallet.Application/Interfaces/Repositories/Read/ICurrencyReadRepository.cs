using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.Currency;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Read;

public interface ICurrencyReadRepository : IBaseContextRepository
{
    Task<Result<GetAllCurrenciesResponseDto>> GetAllCurrenciesAsync(GetAllCurrenciesRequestDto request, CancellationToken cancellationToken = default);

    Task<Result<GetCurrencyByIdResponseDto>> GetCurrencyByIdAsync(GetCurrencyByIdRequestDto request, CancellationToken cancellationToken = default);
}