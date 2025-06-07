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
        , IHttpContextAccessor httpContextAccessor) : CommandHandler<RefreshTokenCommandRequest, RefreshTokenCommandReponse>(httpContextAccessor)
{
    public override async Task<Result<RefreshTokenCommandReponse>> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        //Get r and i from HttpOnly cookies
        string? refreshToken = GetCookie("r");
        string? userId = GetCookie("i");

        if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(userId))
        {
            return Failure(Domain.Errors.Token.InvalidSessionInfo);
        }

        var userResult = await userRepository.GetUserByIdAsync(new(Guid.Parse(userId)));
        if (userResult.IsFailure)
        {
            return Failure(userResult);
        }

        var matchResult = await tokenRepository.MatchTokenAsync(new MatchRefreshTokenRepositoryRequest(userResult.Value.User, refreshToken));
        if (matchResult.IsFailure)
        {
            return Failure(matchResult);
        }

        var accessTokenResult = tokenService.GenerateAccessToken(GenerateTokenServiceRequest.CastFrom(userResult.Value));
        if (accessTokenResult.IsFailure)
        {
            return Failure(accessTokenResult);
        }

        var refreshTokenResult = await tokenRepository.GenerateRefreshTokenAsync(new GenerateRefreshTokenRepositoryRequest(userResult.Value.User));
        if (refreshTokenResult.IsFailure)
        {
            return Failure(refreshTokenResult);
        }

        AppendCookie("r", refreshTokenResult.Value.RefreshToken);

        return Result.Success(new RefreshTokenCommandReponse(accessTokenResult.Value.AccessToken));
    }
}

