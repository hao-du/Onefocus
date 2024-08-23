using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;
using Onefocus.Wallet.Infrastructure.Repositories.Read;

namespace Onefocus.Wallet.Infrastructure.Repositories.Write;

public interface IUserWriteRepository
{
    Task<Result<UpsertUserRepositoryResponse>> UpsertUserAsync(UpsertUserRepositoryRequest request);
}

public sealed class UserWriteRepository : IUserWriteRepository
{
    private readonly WalletWriteDbContext _context;

    private readonly ILogger<UserWriteRepository> _logger;

    public UserWriteRepository(
        ILogger<UserWriteRepository> logger
        , WalletWriteDbContext context
    )
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<UpsertUserRepositoryResponse>> UpsertUserAsync(UpsertUserRepositoryRequest request)
    {
        try
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == request.Id);
            if (user == null)
            {
                var userResult = request.ToRequestObject();
                if (userResult.IsFailure)
                {
                    return Result.Failure<UpsertUserRepositoryResponse>(userResult.Error);
                }
                user = userResult.Value;

                _context.Add(user);
            }
            else
            {
                user.Update(request.Email, request.FirstName, request.LastName, request.Description, request.ActionFlag, request.ActionBy);
            }

            _context.SaveChanges();

            return Result.Success<UpsertUserRepositoryResponse>(new(user.Id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<UpsertUserRepositoryResponse>(CommonErrors.InternalServer);
        }
    }
}