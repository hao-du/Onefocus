using Microsoft.IdentityModel.Tokens;
using Onefocus.Common.Configurations;
using Onefocus.Common.Results;
using Onefocus.Common.Security;
using Onefocus.Common.Utilities;
using Onefocus.Identity.Application.Contracts.Services.Token;
using Onefocus.Identity.Application.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Onefocus.Identity.Infrastructure.Services
{
    public sealed class TokenService(
        IAuthenticationSettings authenticationSettings) : ITokenService
    {
        public Result<GenerateTokenResponseDto> GenerateAccessToken(GenerateTokenRequestDto request)
        {
            if (string.IsNullOrEmpty(request.User.Email))
            {
                return Result.Failure<GenerateTokenResponseDto>(Errors.TokenService.EmailRequired);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, request.User.Email),
                new(ClaimTypes.NameIdentifier, request.User.MembershipUserId.ToString())
            };

            var expiresIn = DateTimeExtensions.Now().AddSeconds(authenticationSettings.AuthTokenExpirySpanSeconds).UtcDateTime;

            var token = new JwtSecurityToken(
                issuer: authenticationSettings.Issuer,
                audience: authenticationSettings.Audience,
                expires: expiresIn,
                claims: claims,
                signingCredentials: new SigningCredentials(Cryptography.CreateSymmetricSecurityKey(authenticationSettings.SymmetricSecurityKey), SecurityAlgorithms.HmacSha256)
            );

            return Result.Success<GenerateTokenResponseDto>(new(new JwtSecurityTokenHandler().WriteToken(token), expiresIn));
        }
    }
}
