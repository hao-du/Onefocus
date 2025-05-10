using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Read.Currency;

namespace Onefocus.Wallet.Domain.Repositories.Read;

public interface ICurrencyReadRepository
{
    Task<Result<GetAllCurrenciesResponseDto>> GetAllCurrenciesAsync(CancellationToken cancellationToken = default);

    Task<Result<GetCurrencyByIdResponseDto>> GetCurrencyByIdAsync(GetCurrencyByIdRequestDto request, CancellationToken cancellationToken = default);

    Task<Result<GetCurrenciesBySpecificationResponseDto>> GetCurrencyBySpecificationAsync(GetCurrenciesBySpecificationRequestDto request, CancellationToken cancellationToken = default);
}