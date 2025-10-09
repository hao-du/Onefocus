using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Identity.Application.Interfaces.Repositories;
using Onefocus.Identity.Application.Interfaces.Services;

namespace Onefocus.Identity.Application.UseCases.Authentication.Commands;

public sealed record AuthenticateCommandRequest(string Email, string Password) : ICommand<AuthenticateCommandResponse>;
public sealed record AuthenticateCommandResponse(string Token, DateTime ExpiresAtUtc);

internal sealed class AuthenticateCommandHandler(
    ILogger<AuthenticateCommandHandler> logger
        , ITokenService tokenService
        , IUserRepository userRepository
        , ITokenRepository tokenRepository
        , IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<AuthenticateCommandRequest, AuthenticateCommandResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<AuthenticateCommandResponse>> Handle(AuthenticateCommandRequest request, CancellationToken cancellationToken = default)
    {
        var checkPasswordResult = await userRepository.CheckPasswordAsync(new(request.Email, request.Password), cancellationToken);
        if (checkPasswordResult.IsFailure)
        {
            return Failure(checkPasswordResult);
        }

        var user = checkPasswordResult.Value.User;

        var accessTokenResult = tokenService.GenerateAccessToken(new(user));
        if (accessTokenResult.IsFailure) return Failure(accessTokenResult);

        var refreshTokenResult = await tokenRepository.GenerateRefreshTokenAsync(new(user), cancellationToken);
        if (refreshTokenResult.IsFailure) return Failure(refreshTokenResult);

        AppendCookie("r", refreshTokenResult.Value.RefreshToken);
        AppendCookie("i", checkPasswordResult.Value.User.Id.ToString());

        return Result.Success(new AuthenticateCommandResponse(accessTokenResult.Value.AccessToken, accessTokenResult.Value.ExpiresAtUtc));
    }
}

