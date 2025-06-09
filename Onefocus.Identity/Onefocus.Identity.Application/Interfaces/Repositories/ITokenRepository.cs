using Onefocus.Common.Results;
using Onefocus.Identity.Application.Contracts.Repositories.Token;

namespace Onefocus.Identity.Application.Interfaces.Repositories;
public interface ITokenRepository
{
    Task<Result<GenerateRefreshTokenResponseDto>> GenerateRefreshTokenAsync(GenerateRefreshTokenRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<GetRefreshTokenResponseDto>> GetRefreshTokenAsync(GetRefreshTokenRequestDto request, CancellationToken cancellationToken = default);
}
