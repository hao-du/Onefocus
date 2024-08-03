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
    Task<Result<GenerateRefreshTokenRepositoryResponse>> GenerateRefreshTokenAsync(GenerateRefreshTokenRepositoryRequest request);
    Task<Result> MatchTokenAsync(MatchRefreshTokenRepositoryRequest request);
}

public sealed class TokenRepository : ITokenRepository
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UserRepository> _logger;

    public TokenRepository(UserManager<User> userManager
        , ILogger<UserRepository> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    public async Task<Result<GenerateRefreshTokenRepositoryResponse>> GenerateRefreshTokenAsync(GenerateRefreshTokenRepositoryRequest request)
    {
        try
        {
            if (request.User == null)
            {
                return Result.Failure<GenerateRefreshTokenRepositoryResponse>(Errors.User.UserNotExist);
            }

            var refreshToken = await _userManager.GenerateUserTokenAsync(request.User, Commons.TokenProviderName, "RefreshToken");
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Result.Failure<GenerateRefreshTokenRepositoryResponse>(Errors.Token.CannotCreateToken);
            }

            var composedTokenName = $"RefreshToken_{request.User.Email}";

            var removeIdentityResult =  await _userManager.RemoveAuthenticationTokenAsync(request.User, Commons.TokenProviderName, composedTokenName);
            if (!removeIdentityResult.Succeeded)
            {
                removeIdentityResult.ToResult<GenerateRefreshTokenRepositoryResponse>();
            }

            var setIdentityResult = await _userManager.SetAuthenticationTokenAsync(request.User, Commons.TokenProviderName, composedTokenName, refreshToken);
            if (!setIdentityResult.Succeeded)
            {
                setIdentityResult.ToResult<GenerateRefreshTokenRepositoryResponse>();
            }

            return Result.Success<GenerateRefreshTokenRepositoryResponse>(new GenerateRefreshTokenRepositoryResponse(refreshToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<GenerateRefreshTokenRepositoryResponse>(CommonErrors.InternalServer);
        }
    }

    public async Task<Result> MatchTokenAsync(MatchRefreshTokenRepositoryRequest request)
    {
        try
        {
            if (request.User == null)
            {
                return Result.Failure<CheckPasswordRepositoryResponse>(Errors.User.IncorrectUserNameOrPassword);
            }

            var composedTokenName = $"RefreshToken_{request.User.Email}";
            var storedToken = await _userManager.GetAuthenticationTokenAsync(request.User, Commons.TokenProviderName, composedTokenName);

            if(storedToken != request.RefreshToken)
            {
                return Result.Failure(Errors.Token.InvalidRefreshToken);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<GenerateRefreshTokenRepositoryResponse>(CommonErrors.InternalServer);
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