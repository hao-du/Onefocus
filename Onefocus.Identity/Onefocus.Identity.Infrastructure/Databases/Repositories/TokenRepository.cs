using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Constants;
using Onefocus.Common.Results;
using Onefocus.Identity.Application.Contracts.Repositories.Token;
using Onefocus.Identity.Application.Interfaces.Repositories;
using Onefocus.Identity.Domain.Entities;
using DomainErrors = Onefocus.Identity.Domain.Errors;

namespace Onefocus.Identity.Infrastructure.Databases.Repositories;



public sealed class TokenRepository(
    UserManager<User> userManager,
    ILogger<TokenRepository> logger
    ) : BaseIdentityRepository<TokenRepository>(logger), ITokenRepository
{
    public async Task<Result<GenerateRefreshTokenResponseDto>> GenerateRefreshTokenAsync(GenerateRefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync<GenerateRefreshTokenResponseDto>(async () =>
        {
            var refreshToken = await userManager.GenerateUserTokenAsync(request.User, Common.Constants.Common.TokenProviderName, "RefreshToken");
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Result.Failure<GenerateRefreshTokenResponseDto>(DomainErrors.Token.CannotCreateToken);
            }

            var composedTokenName = GetComposedTokenName(request.User.Email);

            var removeIdentityResult = await userManager.RemoveAuthenticationTokenAsync(request.User, Common.Constants.Common.TokenProviderName, composedTokenName);
            if (!removeIdentityResult.Succeeded)
                return GetIdentityErrorResult<GenerateRefreshTokenResponseDto>(removeIdentityResult);

            var setIdentityResult = await userManager.SetAuthenticationTokenAsync(request.User, Common.Constants.Common.TokenProviderName, composedTokenName, refreshToken);
            if (!setIdentityResult.Succeeded)
                return GetIdentityErrorResult<GenerateRefreshTokenResponseDto>(setIdentityResult);

            return Result.Success<GenerateRefreshTokenResponseDto>(new(refreshToken));
        });
    }

    public async Task<Result<GetRefreshTokenResponseDto>> GetRefreshTokenAsync(GetRefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var composedTokenName = GetComposedTokenName(request.User.Email);
            var storedToken = await userManager.GetAuthenticationTokenAsync(request.User, Common.Constants.Common.TokenProviderName, composedTokenName);

            if (string.IsNullOrEmpty(storedToken))
            {
                return Result.Failure<GetRefreshTokenResponseDto>(DomainErrors.Token.InvalidRefreshToken);
            }

            return Result.Success<GetRefreshTokenResponseDto>(new(storedToken));
        });
    }

    private static string GetComposedTokenName(string? email)
    {
        return $"RefreshToken_{email}";
    }
}