using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Configurations;
using Onefocus.Common.Results;
using Onefocus.Identity.Infrastructure.Databases.Repositories;
using Onefocus.Identity.Infrastructure.Security;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using System.Linq;

namespace Onefocus.Identity.Application.Authentication.Commands;

public sealed record AuthenticateCommandRequest(string Email, string Password) : ICommand<AuthenticateCommandResponse>
{
    public CheckPasswordRepositoryRequest ToObject() => new(Email, Password);
}
public sealed record AuthenticateCommandResponse(string Token);

internal sealed class AuthenticateCommandHandler : ICommandHandler<AuthenticateCommandRequest, AuthenticateCommandResponse>
{
    private readonly IAuthenticationSettings _authSettings;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticateCommandHandler(
        IAuthenticationSettings authSettings
        , ITokenService tokenService
        , IUserRepository userRepository
        , ITokenRepository tokenRepository
        , IHttpContextAccessor httpContextAccessor)
    {
        _authSettings = authSettings;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<AuthenticateCommandResponse>> Handle(AuthenticateCommandRequest request, CancellationToken cancellationToken)
    {
        var checkPasswordResult = await _userRepository.CheckPasswordAsync(request.ToObject());
        if (checkPasswordResult.IsFailure)
        {
            return Result.Failure<AuthenticateCommandResponse>(checkPasswordResult.Error);
        }

        var accessTokenResult = _tokenService.GenerateAccessToken(GenerateTokenServiceRequest.CastFrom(checkPasswordResult.Value));
        if (accessTokenResult.IsFailure)
        {
            return Result.Failure<AuthenticateCommandResponse>(accessTokenResult.Error);
        }

        var refreshTokenResult = await _tokenRepository.GenerateRefreshTokenAsync(new GenerateRefreshTokenRepositoryRequest(checkPasswordResult.Value.User));
        if (refreshTokenResult.IsFailure)
        {
            return Result.Failure<AuthenticateCommandResponse>(refreshTokenResult.Error);
        }

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("r", refreshTokenResult.Value.RefreshToken, new CookieOptions
        {
            SameSite = SameSiteMode.Unspecified,
            HttpOnly = true
        });
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("i", checkPasswordResult.Value.User.Id.ToString(), new CookieOptions
        {
            SameSite = SameSiteMode.Unspecified,
            HttpOnly = true
        });

        return Result.Success(new AuthenticateCommandResponse(accessTokenResult.Value.AccessToken));
    }
}

