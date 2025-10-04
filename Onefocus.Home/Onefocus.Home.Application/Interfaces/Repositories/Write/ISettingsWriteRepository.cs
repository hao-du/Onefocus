using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Contracts.Write.Settings;

namespace Onefocus.Home.Application.Interfaces.Repositories.Write;

public interface ISettingsWriteRepository : IBaseContextRepository
{
    Task<Result<GetSettingsByUserIdResponseDto>> GetSettingsByUserIdAsync(GetSettingsByUserIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddSettingsAsync(CreateSettingsRequestDto request, CancellationToken cancellationToken = default);
}