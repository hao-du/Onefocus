using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Identity.Application.Interfaces.Repositories;
using Onefocus.Identity.Application.Interfaces.Services;
using DomainErrors = Onefocus.Identity.Domain.Errors;

namespace Onefocus.Identity.Application.UseCases.Authentication.Commands;

public sealed record RefreshTokenCommandRequest() : ICommand<RefreshTokenCommandReponse>;
public sealed record RefreshTokenCommandReponse(string Token);

internal sealed class RefreshTokenCommandHandler(
    ITokenService tokenService
        , IUserRepository userRepository
        , ITokenRepository tokenRepository
        , IHttpContextAccessor httpContextAccessor) : CommandHandler<RefreshTokenCommandRequest, RefreshTokenCommandReponse>(httpContextAccessor)
{
    public override async Task<Result<RefreshTokenCommandReponse>> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken = default)
    {
        //Get r and i from HttpOnly cookies
        string? cookieRefreshToken = GetCookie("r");
        string? cookieUserId = GetCookie("i");
        if (string.IsNullOrEmpty(cookieRefreshToken) || string.IsNullOrEmpty(cookieUserId)) return Failure(DomainErrors.Token.InvalidSessionInfo);

        var userResult = await userRepository.GetUserByIdAsync(new(Guid.Parse(cookieUserId)), cancellationToken);
        if (userResult.IsFailure) return Failure(userResult);
        if (userResult.Value.User == null) return Result.Failure<RefreshTokenCommandReponse>(DomainErrors.User.UserNotExist);

        var user = userResult.Value.User;

        //Get fresh token from repository
        var getRefreshTokenResult = await tokenRepository.GetRefreshTokenAsync(new(user), cancellationToken);
        if (getRefreshTokenResult.IsFailure) return Failure(getRefreshTokenResult);

        //Compare cookie refresh token with the one from repository
        if (cookieRefreshToken != getRefreshTokenResult.Value.RefreshToken)
            return Result.Failure<RefreshTokenCommandReponse>(DomainErrors.Token.InvalidRefreshToken);

        //Generate new access and refresh tokens
        var accessTokenResult = tokenService.GenerateAccessToken(new(user));
        if (accessTokenResult.IsFailure) return Failure(accessTokenResult);

        var refreshTokenResult = await tokenRepository.GenerateRefreshTokenAsync(new(user), cancellationToken);
        if (refreshTokenResult.IsFailure) return Failure(refreshTokenResult);

        AppendCookie("r", refreshTokenResult.Value.RefreshToken);

        return Result.Success<RefreshTokenCommandReponse>(new(accessTokenResult.Value.AccessToken));
    }
}

