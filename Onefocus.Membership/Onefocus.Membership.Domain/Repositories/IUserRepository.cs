using Onefocus.Common.Results;
using Onefocus.Membership.Domain.Messages;

namespace Onefocus.Membership.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Result<GetAllUsersResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default);
        Task<Result> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);
        Task<Result> UpdateUserAsync(UpdateUserRequestDto request, CancellationToken cancellationToken = default);
    }
}
