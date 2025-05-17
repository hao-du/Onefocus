using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Configurations;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Identity.Infrastructure.Databases.Repositories;
using Onefocus.Identity.Infrastructure.Security;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using System.Linq;

namespace Onefocus.Identity.Application.Authentication.Commands;

public sealed record RefreshTokenCommandRequest() : ICommand<RefreshTokenCommandReponse>;
public sealed record RefreshTokenCommandReponse(string Token);

internal sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommandRequest, RefreshTokenCommandReponse>
{
    private readonly IAuthenticationSettings _authSettings;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RefreshTokenCommandHandler(
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

    public async Task<Result<RefreshTokenCommandReponse>> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        //Get r and i from HttpOnly cookies
        string? refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["r"];
        string? userId = _httpContextAccessor.HttpContext?.Request.Cookies["i"];

        if(string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(userId))
        {
            return Result.Failure<RefreshTokenCommandReponse>(Domain.Errors.Token.InvalidSessionInfo);
        }

        var userResult = await _userRepository.GetUserByIdAsync(new (Guid.Parse(userId)));
        if (userResult.IsFailure)
        {
            return Result.Failure<RefreshTokenCommandReponse>(userResult.Error);
        }

        var matchResult = await _tokenRepository.MatchTokenAsync(new MatchRefreshTokenRepositoryRequest(userResult.Value.User, refreshToken));
        if (matchResult.IsFailure)
        {
            return Result.Failure<RefreshTokenCommandReponse>(matchResult.Error);
        }

        var accessTokenResult = _tokenService.GenerateAccessToken(GenerateTokenServiceRequest.CastFrom(userResult.Value));
        if (accessTokenResult.IsFailure)
        {
            return Result.Failure<RefreshTokenCommandReponse>(accessTokenResult.Error);
        }

        var refreshTokenResult = await _tokenRepository.GenerateRefreshTokenAsync(new GenerateRefreshTokenRepositoryRequest(userResult.Value.User));
        if (refreshTokenResult.IsFailure)
        {
            return Result.Failure<RefreshTokenCommandReponse>(refreshTokenResult.Error);
        }

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("r", refreshTokenResult.Value.RefreshToken, new CookieOptions
        {
            SameSite = SameSiteMode.Unspecified,
            HttpOnly = true
        });

        return Result.Success(new RefreshTokenCommandReponse(accessTokenResult.Value.AccessToken));
    }
}

