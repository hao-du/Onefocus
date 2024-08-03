﻿using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Identity.Infrastructure.Databases.Repositories;
using Onefocus.Identity.Infrastructure.Security;
using Onefocus.Membership.Api.Security;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using System.Linq;

namespace Onefocus.Identity.Application.Authentication.Queries;

public sealed record AuthenticateQueryRequest(string Email, string Password) : IQuery<AccessTokenResponse>, IRequestObject<CheckPasswordRepositoryRequest>
{
    public CheckPasswordRepositoryRequest ToRequestObject() => new(Email, Password);
}

internal sealed class AuthenticateQueryHandler : IQueryHandler<AuthenticateQueryRequest, AccessTokenResponse>
{
    private readonly IAuthenticationSettings _authSettings;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;

    public AuthenticateQueryHandler(
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

    public async Task<Result<AccessTokenResponse>> Handle(AuthenticateQueryRequest request, CancellationToken cancellationToken)
    {
        var checkPasswordResult = await _userRepository.CheckPasswordAsync(request.ToRequestObject());
        if (checkPasswordResult.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(checkPasswordResult.Error);
        }

        var accessTokenResult = _tokenService.GenerateAccessToken(GenerateTokenServiceRequest.Create(checkPasswordResult.Value));
        if (accessTokenResult.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(checkPasswordResult.Error);
        }

        var refreshTokenResult = await _tokenRepository.GenerateRefreshTokenAsync(new UpsertTokenRepositoryRequest(checkPasswordResult.Value.Email));
        if (refreshTokenResult.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(checkPasswordResult.Error);
        }

        var token = new AccessTokenResponse()
        {
            AccessToken = accessTokenResult.Value.AccessToken,
            RefreshToken = refreshTokenResult.Value.RefreshToken,
            ExpiresIn = _authSettings.AuthTokenExpirySpanSeconds
        };

        return Result.Success<AccessTokenResponse>(token);
    }
}

