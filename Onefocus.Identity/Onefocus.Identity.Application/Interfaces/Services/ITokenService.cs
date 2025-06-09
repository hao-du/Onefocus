using Onefocus.Common.Results;
using Onefocus.Identity.Application.Contracts.Services.Token;

namespace Onefocus.Identity.Application.Interfaces.Services;

public interface ITokenService
{
    Result<GenerateTokenResponseDto> GenerateAccessToken(GenerateTokenRequestDto request);
}
