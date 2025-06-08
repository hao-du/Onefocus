using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Domain.Messages.Write.User;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Wallet.Infrastructure.Repositories.Write;

public sealed class UserWriteRepository(
    ILogger<UserWriteRepository> logger
        , WalletWriteDbContext context
    ) : BaseContextRepository<UserWriteRepository>(logger, context), IUserWriteRepository
{
    public async Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var user = await context.User.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            return Result.Success<GetUserByIdResponseDto>(new(user));
        });
    }

    public async Task<Result> AddUserAsync(AddUserRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddAsync(request.User);
            return Result.Success();
        });
    }
}