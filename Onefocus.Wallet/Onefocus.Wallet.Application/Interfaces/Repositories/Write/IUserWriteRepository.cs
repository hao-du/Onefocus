using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Write.User;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Write;

public interface IUserWriteRepository : IBaseContextRepository
{
    Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddUserAsync(AddUserRequestDto request, CancellationToken cancellationToken = default);
}