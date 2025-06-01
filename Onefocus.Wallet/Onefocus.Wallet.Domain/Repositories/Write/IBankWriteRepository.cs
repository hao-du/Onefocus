using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Write.Bank;

namespace Onefocus.Wallet.Domain.Repositories.Write;

public interface IBankWriteRepository : IBaseContextRepository
{
    Task<Result<GetBankByIdResponseDto>> GetBankByIdAsync(GetBankByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddBankAsync(CreateBankRequestDto request, CancellationToken cancellationToken = default);
}