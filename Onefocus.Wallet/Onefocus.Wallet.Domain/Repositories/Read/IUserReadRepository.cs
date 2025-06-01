using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Read.User;

namespace Onefocus.Wallet.Domain.Repositories.Read;

public interface IUserReadRepository : IBaseContextRepository
{
    Task<Result<GetAllUsersResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default);
}