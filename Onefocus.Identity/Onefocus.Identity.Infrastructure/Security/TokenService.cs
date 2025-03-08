﻿using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Onefocus.Common.Results;
using Onefocus.Common.Security;
using Onefocus.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Identity.Infrastructure.Security
{
    public interface ITokenService
    {
        Result<GenerateTokenServiceResponse> GenerateAccessToken(GenerateTokenServiceRequest request);
    }

    public sealed class TokenService : ITokenService
    {
        private readonly IAuthenticationSettings _authenticationSettings;
        private readonly SymmetricSecurityKey _symmetricSecurityKey;
        private readonly UserManager<User> _userManager;

        public TokenService(
            IAuthenticationSettings authenticationSettings
            , SymmetricSecurityKey symmetricSecurityKey
            , UserManager<User> userManager)
        {
            _authenticationSettings = authenticationSettings;
            _symmetricSecurityKey = symmetricSecurityKey;
            _userManager = userManager;
        }

        public Result<GenerateTokenServiceResponse> GenerateAccessToken(GenerateTokenServiceRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return Result.Failure<GenerateTokenServiceResponse>(Errors.TokenService.EmailRequired);
            }

            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, request.Email));

            var token = new JwtSecurityToken(
                issuer: _authenticationSettings.Issuer,
                audience: _authenticationSettings.Audience,
                expires: DateTime.UtcNow.AddSeconds(_authenticationSettings.AuthTokenExpirySpanSeconds),
                claims: claims,
                signingCredentials: new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
            );

            return Result.Success<GenerateTokenServiceResponse>(new (new JwtSecurityTokenHandler().WriteToken(token)));
        }
    }
}
