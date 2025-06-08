using Onefocus.Common.Results;
using Onefocus.Membership.Application.Contracts;

namespace Onefocus.Membership.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<Result<GetAllUsersResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default);
        Task<Result> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);
        Task<Result> UpdateUserAsync(UpdateUserRequestDto request, CancellationToken cancellationToken = default);
    }
}
