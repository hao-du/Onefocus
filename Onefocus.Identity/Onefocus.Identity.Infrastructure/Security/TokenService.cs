using Microsoft.IdentityModel.Tokens;
using Onefocus.Common.Configurations;
using Onefocus.Common.Results;
using Onefocus.Common.Security;
using Onefocus.Common.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Onefocus.Identity.Infrastructure.Security
{
    public interface ITokenService
    {
        Result<GenerateTokenServiceResponse> GenerateAccessToken(GenerateTokenServiceRequest request);
    }

    public sealed class TokenService(
        IAuthenticationSettings authenticationSettings) : ITokenService
    {
        public Result<GenerateTokenServiceResponse> GenerateAccessToken(GenerateTokenServiceRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return Result.Failure<GenerateTokenServiceResponse>(Errors.TokenService.EmailRequired);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, request.Email),
                new(ClaimTypes.NameIdentifier, request.MembershipUserId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: authenticationSettings.Issuer,
                audience: authenticationSettings.Audience,
                expires: DateTimeExtensions.Now().AddSeconds(authenticationSettings.AuthTokenExpirySpanSeconds).UtcDateTime,
                claims: claims,
                signingCredentials: new SigningCredentials(Cryptography.CreateSymmetricSecurityKey(authenticationSettings.SymmetricSecurityKey), SecurityAlgorithms.HmacSha256)
            );

            return Result.Success<GenerateTokenServiceResponse>(new(new JwtSecurityTokenHandler().WriteToken(token)));
        }
    }
}
