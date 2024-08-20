using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts;

namespace Onefocus.Wallet.Infrastructure.Repositories.Read;

public interface IUserReadRepository
{
    Task<Result<GetAllUsersRepositoryResponse>> GetAllUsersAsync();
    Task<Result<GetUserByIdRepositoryResponse?>> GetUserByIdAsync(GetUserByIdRepositoryRequest request);
}

public sealed class UserReadRepository : IUserReadRepository
{
    private readonly WalletReadDbContext _context;

    private readonly ILogger<UserReadRepository> _logger;

    public UserReadRepository(
        ILogger<UserReadRepository> logger
        , WalletReadDbContext context
    )
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<GetAllUsersRepositoryResponse>> GetAllUsersAsync()
    {
        try
        {
            var users = await _context.User.Where(u => u.ActiveFlag).ToListAsync();

            return Result.Success(GetAllUsersRepositoryResponse.Create(users));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<GetAllUsersRepositoryResponse>(CommonErrors.InternalServer);
        }
    }

    public async Task<Result<GetUserByIdRepositoryResponse?>> GetUserByIdAsync(GetUserByIdRepositoryRequest request)
    {
        try
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == request.Id);

            return Result.Success(GetUserByIdRepositoryResponse.Create(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<GetUserByIdRepositoryResponse?>(CommonErrors.InternalServer);
        }
    }
}