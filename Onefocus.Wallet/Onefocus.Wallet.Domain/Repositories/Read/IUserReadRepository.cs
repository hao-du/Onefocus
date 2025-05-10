using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Read;

namespace Onefocus.Wallet.Domain.Repositories.Read;

public interface IUserReadRepository
{
    Task<Result<GetAllUsersResponseDto>> GetAllUsersAsync();
    Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request);
}