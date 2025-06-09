using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Write.Bank;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Write;

public interface IBankWriteRepository : IBaseContextRepository
{
    Task<Result<GetBankByIdResponseDto>> GetBankByIdAsync(GetBankByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddBankAsync(CreateBankRequestDto request, CancellationToken cancellationToken = default);
}