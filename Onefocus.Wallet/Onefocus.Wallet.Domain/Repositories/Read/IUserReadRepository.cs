using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Read;

namespace Onefocus.Wallet.Domain.Repositories.Read;

public interface IUserReadRepository
{
    Task<Result<GetAllUsersResponse>> GetAllUsersAsync();
    Task<Result<GetUserByIdResponse>> GetUserByIdAsync(GetUserByIdRequest request);
}