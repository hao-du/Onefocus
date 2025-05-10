using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Read;
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

    public async Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request)
    {
        return await ExecuteAsync<GetUserByIdResponseDto>(async () =>
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == request.Id);
            return Result.Success(GetUserByIdResponseDto.CastFrom(user));
        });
    }

    public async Task<Result<GetAllUsersResponseDto>> GetAllUsersAsync()
    {
        return await ExecuteAsync(async () =>
        {
            var users = await _context.User.Where(u => u.ActiveFlag).ToListAsync();
            return Result.Success(GetAllUsersResponseDto.CastFrom(users));
        });
    }
}