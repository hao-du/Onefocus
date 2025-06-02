using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Identity.Infrastructure.Databases.Repositories;
using Onefocus.Identity.Infrastructure.Security;

namespace Onefocus.Identity.Application.Authentication.Commands;

public sealed record RefreshTokenCommandRequest() : ICommand<RefreshTokenCommandReponse>;
public sealed record RefreshTokenCommandReponse(string Token);

internal sealed class RefreshTokenCommandHandler(
    ITokenService tokenService
        , IUserRepository userRepository
        , ITokenRepository tokenRepository
        , IHttpContextAccessor httpContextAccessor) : ICommandHandler<RefreshTokenCommandRequest, RefreshTokenCommandReponse>
{
    public async Task<Result<RefreshTokenCommandReponse>> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        //Get r and i from HttpOnly cookies
        string? refreshToken = httpContextAccessor.HttpContext?.Request.Cookies["r"];
        string? userId = httpContextAccessor.HttpContext?.Request.Cookies["i"];

        if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(userId))
        {
            return Result.Failure<RefreshTokenCommandReponse>(Domain.Errors.Token.InvalidSessionInfo);
        }

        var userResult = await userRepository.GetUserByIdAsync(new(Guid.Parse(userId)));
        if (userResult.IsFailure)
        {
            return Result.Failure<RefreshTokenCommandReponse>(userResult.Errors);
        }

        var matchResult = await tokenRepository.MatchTokenAsync(new MatchRefreshTokenRepositoryRequest(userResult.Value.User, refreshToken));
        if (matchResult.IsFailure)
        {
            return Result.Failure<RefreshTokenCommandReponse>(matchResult.Errors);
        }

        var accessTokenResult = tokenService.GenerateAccessToken(GenerateTokenServiceRequest.CastFrom(userResult.Value));
        if (accessTokenResult.IsFailure)
        {
            return Result.Failure<RefreshTokenCommandReponse>(accessTokenResult.Errors);
        }

        var refreshTokenResult = await tokenRepository.GenerateRefreshTokenAsync(new GenerateRefreshTokenRepositoryRequest(userResult.Value.User));
        if (refreshTokenResult.IsFailure)
        {
            return Result.Failure<RefreshTokenCommandReponse>(refreshTokenResult.Errors);
        }

        httpContextAccessor.HttpContext?.Response.Cookies.Append("r", refreshTokenResult.Value.RefreshToken, new CookieOptions
        {
            SameSite = SameSiteMode.Unspecified,
            HttpOnly = true
        });

        return Result.Success(new RefreshTokenCommandReponse(accessTokenResult.Value.AccessToken));
    }
}

