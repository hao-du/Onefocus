using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Onefocus.Common.Configurations;
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
        private readonly UserManager<User> _userManager;

        public TokenService(
            IAuthenticationSettings authenticationSettings
            , UserManager<User> userManager)
        {
            _authenticationSettings = authenticationSettings;
            _userManager = userManager;
        }

        public Result<GenerateTokenServiceResponse> GenerateAccessToken(GenerateTokenServiceRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return Result.Failure<GenerateTokenServiceResponse>(Errors.TokenService.EmailRequired);
            }

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, request.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, request.MembershipUserId.ToString()));

            var token = new JwtSecurityToken(
                issuer: _authenticationSettings.Issuer,
                audience: _authenticationSettings.Audience,
                expires: DateTime.UtcNow.AddSeconds(_authenticationSettings.AuthTokenExpirySpanSeconds),
                claims: claims,
                signingCredentials: new SigningCredentials(Cryptography.CreateSymmetricSecurityKey(_authenticationSettings.SymmetricSecurityKey), SecurityAlgorithms.HmacSha256)
            );

            return Result.Success<GenerateTokenServiceResponse>(new (new JwtSecurityTokenHandler().WriteToken(token)));
        }
    }
}
