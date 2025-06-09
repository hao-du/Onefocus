using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.User;
using Onefocus.Wallet.Application.Interfaces.Repositories.Read;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Wallet.Infrastructure.Repositories.Read;

public sealed class UserReadRepository(
    ILogger<UserReadRepository> logger
        , WalletReadDbContext context
    ) : BaseContextRepository<UserReadRepository>(logger, context), IUserReadRepository
{
    public async Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var user = await context.User.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            return Result.Success<GetUserByIdResponseDto>(new(user));
        });
    }

    public async Task<Result<GetAllUsersResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var users = await context.User.Where(u => u.IsActive).ToListAsync(cancellationToken);
            return Result.Success<GetAllUsersResponseDto>(new(users));
        });
    }
}