using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.User;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Read;

public interface IUserReadRepository : IBaseContextRepository
{
    Task<Result<GetAllUsersResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default);
}