using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Write.User;

namespace Onefocus.Wallet.Domain.Repositories.Write;

public interface IUserWriteRepository
{
    Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddUserAsync(AddUserRequestDto request, CancellationToken cancellationToken = default);
}