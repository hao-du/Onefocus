using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Contracts.Read.Settings;

namespace Onefocus.Home.Application.Interfaces.Repositories.Read;

public interface ISettingsReadRepository : IBaseContextRepository
{
    Task<Result<GetSettingsByUserIdResponseDto>> GetSettingsByUserIdAsync(GetSettingsByUserIdRequestDto request, CancellationToken cancellationToken = default);
}