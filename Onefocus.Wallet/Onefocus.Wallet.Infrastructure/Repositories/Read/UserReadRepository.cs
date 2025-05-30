using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Read.User;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Wallet.Domain.Repositories.Read;

public sealed class UserReadRepository : BaseRepository<UserReadRepository>, IUserReadRepository
{
    private readonly WalletReadDbContext _context;

    public UserReadRepository(
        ILogger<UserReadRepository> logger
        , WalletReadDbContext context
    ) : base(logger)
    {
        _context = context;
    }

    public async Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync<GetUserByIdResponseDto>(async () =>
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            return Result.Success<GetUserByIdResponseDto>(new(user));
        });
    }

    public async Task<Result<GetAllUsersResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var users = await _context.User.Where(u => u.IsActive).ToListAsync(cancellationToken);
            return Result.Success<GetAllUsersResponseDto>(new(users));
        });
    }
}