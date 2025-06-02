using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Identity.Infrastructure.Databases.Repositories;
using Onefocus.Identity.Infrastructure.Security;

namespace Onefocus.Identity.Application.Authentication.Commands;

public sealed record AuthenticateCommandRequest(string Email, string Password) : ICommand<AuthenticateCommandResponse>
{
    public CheckPasswordRepositoryRequest ToObject() => new(Email, Password);
}
public sealed record AuthenticateCommandResponse(string Token);

internal sealed class AuthenticateCommandHandler(
    ITokenService tokenService
        , IUserRepository userRepository
        , ITokenRepository tokenRepository
        , IHttpContextAccessor httpContextAccessor) : ICommandHandler<AuthenticateCommandRequest, AuthenticateCommandResponse>
{
    public async Task<Result<AuthenticateCommandResponse>> Handle(AuthenticateCommandRequest request, CancellationToken cancellationToken)
    {
        var checkPasswordResult = await userRepository.CheckPasswordAsync(request.ToObject());
        if (checkPasswordResult.IsFailure)
        {
            return Result.Failure<AuthenticateCommandResponse>(checkPasswordResult.Errors);
        }

        var accessTokenResult = tokenService.GenerateAccessToken(GenerateTokenServiceRequest.CastFrom(checkPasswordResult.Value));
        if (accessTokenResult.IsFailure)
        {
            return Result.Failure<AuthenticateCommandResponse>(accessTokenResult.Errors);
        }

        var refreshTokenResult = await tokenRepository.GenerateRefreshTokenAsync(new GenerateRefreshTokenRepositoryRequest(checkPasswordResult.Value.User));
        if (refreshTokenResult.IsFailure)
        {
            return Result.Failure<AuthenticateCommandResponse>(refreshTokenResult.Errors);
        }

        httpContextAccessor.HttpContext?.Response.Cookies.Append("r", refreshTokenResult.Value.RefreshToken, new CookieOptions
        {
            SameSite = SameSiteMode.Unspecified,
            HttpOnly = true
        });
        httpContextAccessor.HttpContext?.Response.Cookies.Append("i", checkPasswordResult.Value.User.Id.ToString(), new CookieOptions
        {
            SameSite = SameSiteMode.Unspecified,
            HttpOnly = true
        });

        return Result.Success(new AuthenticateCommandResponse(accessTokenResult.Value.AccessToken));
    }
}

