using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Read.Bank;

namespace Onefocus.Wallet.Domain.Repositories.Read;

public interface IBankReadRepository : IBaseContextRepository
{
    Task<Result<GetAllBanksResponseDto>> GetAllBanksAsync(CancellationToken cancellationToken = default);

    Task<Result<GetBankByIdResponseDto>> GetBankByIdAsync(GetBankByIdRequestDto request, CancellationToken cancellationToken = default);

}