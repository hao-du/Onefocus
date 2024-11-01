using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Common.Security;
using Onefocus.Identity.Infrastructure.Databases.Repositories;
using Onefocus.Identity.Infrastructure.Security;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using System.Linq;

namespace Onefocus.Identity.Application.Authentication.Commands;

public sealed record RefreshTokenCommandRequest(Guid UserId, string RefreshToken) : ICommand<AccessTokenResponse>;

internal sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommandRequest, AccessTokenResponse>
{
    private readonly IAuthenticationSettings _authSettings;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;

    public RefreshTokenCommandHandler(
        IAuthenticationSettings authSettings
        , ITokenService tokenService
        , IUserRepository userRepository
        , ITokenRepository tokenRepository)
    {
        _authSettings = authSettings;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
    }

    public async Task<Result<AccessTokenResponse>> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetUserByIdAsync(new (request.UserId));
        if (userResult.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(userResult.Error);
        }

        var matchResult = await _tokenRepository.MatchTokenAsync(new MatchRefreshTokenRepositoryRequest(userResult.Value.User, request.RefreshToken));
        if (matchResult.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(matchResult.Error);
        }

        var accessTokenResult = _tokenService.GenerateAccessToken(GenerateTokenServiceRequest.Cast(userResult.Value));
        if (accessTokenResult.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(accessTokenResult.Error);
        }

        var refreshTokenResult = await _tokenRepository.GenerateRefreshTokenAsync(new GenerateRefreshTokenRepositoryRequest(userResult.Value.User));
        if (refreshTokenResult.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(refreshTokenResult.Error);
        }

        var token = new AccessTokenResponse()
        {
            AccessToken = accessTokenResult.Value.AccessToken,
            RefreshToken = refreshTokenResult.Value.RefreshToken,
            ExpiresIn = _authSettings.AuthTokenExpirySpanSeconds
        };

        return Result.Success(token);
    }
}

