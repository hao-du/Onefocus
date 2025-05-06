using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Wallet.Domain.Messages.Write;
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

    public async Task<Result<UpsertUserResponse>> UpsertUserAsync(UpsertUserRequest request)
    {
        return await ExecuteAsync(async () =>
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == request.Id);
            if (user == null)
            {
                var userResult = request.ToObject();
                if (userResult.IsFailure)
                {
                    return Result.Failure<UpsertUserResponse>(userResult.Error);
                }
                user = userResult.Value;

                await _context.AddAsync(user);
            }
            else
            {
                user.Update(request.Email, request.FirstName, request.LastName, request.Description, request.ActionFlag, request.ActionBy);
            }

            await _context.SaveChangesAsync();

            return Result.Success<UpsertUserResponse>(new(user.Id));
        });
    }
}