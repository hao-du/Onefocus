using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Write.Currency;

namespace Onefocus.Wallet.Domain.Repositories.Write;

public interface ICurrencyWriteRepository : IBaseContextRepository
{
    Task<Result<GetCurrencyByIdResponseDto>> GetCurrencyByIdAsync(GetCurrencyByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddCurrencyAsync(CreateCurrencyRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> BulkMarkDefaultFlag(BulkMarkDefaultFlagRequestDto request, CancellationToken cancellationToken = default);
}