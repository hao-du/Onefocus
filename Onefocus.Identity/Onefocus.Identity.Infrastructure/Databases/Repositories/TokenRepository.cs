using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Constants;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Identity.Domain;
using Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.Databases.Repositories;

public interface ITokenRepository
{
    Task<Result<GenerateRefreshTokenRepositoryResponse>> GenerateRefreshTokenAsync(GenerateRefreshTokenRepositoryRequest request);
    Task<Result> MatchTokenAsync(MatchRefreshTokenRepositoryRequest request);
}

public sealed class TokenRepository(
    UserManager<User> userManager,
    ILogger<TokenRepository> logger
    ) : BaseRepository<TokenRepository>(logger), ITokenRepository
{
    public async Task<Result<GenerateRefreshTokenRepositoryResponse>> GenerateRefreshTokenAsync(GenerateRefreshTokenRepositoryRequest request)
    {
        return await ExecuteAsync<GenerateRefreshTokenRepositoryResponse>(async () =>
        {
            if (request.User == null)
            {
                return Result.Failure<GenerateRefreshTokenRepositoryResponse>(Errors.User.UserNotExist);
            }

            var refreshToken = await userManager.GenerateUserTokenAsync(request.User, Commons.TokenProviderName, "RefreshToken");
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Result.Failure<GenerateRefreshTokenRepositoryResponse>(Errors.Token.CannotCreateToken);
            }

            var composedTokenName = $"RefreshToken_{request.User.Email}";

            var removeIdentityResult = await userManager.RemoveAuthenticationTokenAsync(request.User, Commons.TokenProviderName, composedTokenName);
            if (!removeIdentityResult.Succeeded)
            {
                return removeIdentityResult.ToResult<GenerateRefreshTokenRepositoryResponse>();
            }

            var setIdentityResult = await userManager.SetAuthenticationTokenAsync(request.User, Commons.TokenProviderName, composedTokenName, refreshToken);
            if (!setIdentityResult.Succeeded)
            {
                return setIdentityResult.ToResult<GenerateRefreshTokenRepositoryResponse>();
            }

            return Result.Success<GenerateRefreshTokenRepositoryResponse>(new GenerateRefreshTokenRepositoryResponse(refreshToken));
        });
    }

    public async Task<Result> MatchTokenAsync(MatchRefreshTokenRepositoryRequest request)
    {
        return await ExecuteAsync(async () =>
        {
            if (request.User == null)
            {
                return Result.Failure<CheckPasswordRepositoryResponse>(Errors.User.IncorrectUserNameOrPassword);
            }

            var composedTokenName = $"RefreshToken_{request.User.Email}";
            var storedToken = await userManager.GetAuthenticationTokenAsync(request.User, Commons.TokenProviderName, composedTokenName);

            if (storedToken != request.RefreshToken)
            {
                return Result.Failure(Errors.Token.InvalidRefreshToken);
            }

            return Result.Success();
        });
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