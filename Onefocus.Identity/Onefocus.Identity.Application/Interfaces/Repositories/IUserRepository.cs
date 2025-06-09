using Onefocus.Common.Results;
using Onefocus.Identity.Application.Contracts.Repositories.User;

namespace Onefocus.Identity.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Result<CheckPasswordResponseDto>> CheckPasswordAsync(CheckPasswordRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<GetUserByEmailResponseDto>> GetUserByEmailAsync(GetUserByEmailRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> UpdateUserAsync(UpdateUserRequestDto request, CancellationToken cancellationToken = default);
}
