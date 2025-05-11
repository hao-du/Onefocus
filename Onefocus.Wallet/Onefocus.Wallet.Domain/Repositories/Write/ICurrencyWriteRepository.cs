using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Write;

namespace Onefocus.Wallet.Domain.Repositories.Write;

public interface ICurrencyWriteRepository
{
    Task<Result> AddCurrencyAsync(CreateCurrencyRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> BulkMarkDefaultFlag(BulkMarkDefaultFlagRequestDto request, CancellationToken cancellationToken = default);
}