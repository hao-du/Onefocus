using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Wallet.Domain.Messages.Write.User;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Wallet.Domain.Repositories.Write;

public sealed class UserWriteRepository : BaseRepository<UserWriteRepository>, IUserWriteRepository
{
    private readonly WalletWriteDbContext _context;

    public UserWriteRepository(
        ILogger<UserWriteRepository> logger
        , WalletWriteDbContext context
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

    public async Task<Result> AddUserAsync(AddUserRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await _context.AddAsync(request.User);
            return Result.Success();
        });
    }
}