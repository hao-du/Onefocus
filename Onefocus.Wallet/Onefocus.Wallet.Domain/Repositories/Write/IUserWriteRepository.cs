using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Write.User;

namespace Onefocus.Wallet.Domain.Repositories.Write;

public interface IUserWriteRepository : IBaseContextRepository
{
    Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddUserAsync(AddUserRequestDto request, CancellationToken cancellationToken = default);
}