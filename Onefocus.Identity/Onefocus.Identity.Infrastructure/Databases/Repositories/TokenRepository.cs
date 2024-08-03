using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Constants;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Identity.Domain;
using Onefocus.Identity.Domain.Entities;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Onefocus.Identity.Infrastructure.Databases.Repositories;

public interface ITokenRepository
{
    Task<Result<UpsertTokenRepositoryResponse>> GenerateRefreshTokenAsync(UpsertTokenRepositoryRequest request);
}

public sealed class TokenRepository : ITokenRepository
{
    private readonly UserManager<User> _userManager;
    private readonly IUserStore<User> _userStore;
    private readonly IUserEmailStore<User> _emailStore;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<UserRepository> _logger;

    public TokenRepository(UserManager<User> userManager
        , IUserStore<User> userStore
        , ILogger<UserRepository> logger
        , IPasswordHasher<User> passwordHasher)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = (IUserEmailStore<User>)userStore;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }
    public async Task<Result<UpsertTokenRepositoryResponse>> GenerateRefreshTokenAsync(UpsertTokenRepositoryRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure<UpsertTokenRepositoryResponse>(Errors.User.UserNotExist);
            }

            var refreshToken = await _userManager.GenerateUserTokenAsync(user, Commons.TokenProviderName, "RefreshToken");
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Result.Failure<UpsertTokenRepositoryResponse>(Errors.User.CannotCreateToken);
            }

            string composedTokenName = $"RefreshToken_{user.Email}";
            var removeIdentityResult =  await _userManager.RemoveAuthenticationTokenAsync(user, Commons.TokenProviderName, composedTokenName);
            if (!removeIdentityResult.Succeeded)
            {
                removeIdentityResult.ToResult<UpsertTokenRepositoryResponse>();
            }

            var setIdentityResult = await _userManager.SetAuthenticationTokenAsync(user, Commons.TokenProviderName, composedTokenName, refreshToken);
            if (!setIdentityResult.Succeeded)
            {
                setIdentityResult.ToResult<UpsertTokenRepositoryResponse>();
            }

            return Result.Success<UpsertTokenRepositoryResponse>(new UpsertTokenRepositoryResponse(refreshToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<UpsertTokenRepositoryResponse>(CommonErrors.InternalServer);
        }
    }
}

internal static class IdentityResultExtension
{
    internal static Result<TValue> ToResult<TValue>(this IdentityResult identityResult)
    {
        var identityError = identityResult.Errors.FirstOrDefault();
        if (identityError != null)
        {
            return Result.Failure<TValue>(new Error(identityError.Code, identityError.Description));
        }
        return Result.Failure<TValue>(CommonErrors.Unknown);
    }
}