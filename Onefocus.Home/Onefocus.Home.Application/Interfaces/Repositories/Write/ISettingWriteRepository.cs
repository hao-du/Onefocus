using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Contracts.Write.Setting;

namespace Onefocus.Home.Application.Interfaces.Repositories.Write;

public interface ISettingWriteRepository : IBaseContextRepository
{
    Task<Result<GetSettingByUserIdResponseDto>> GetSettingByUserIdAsync(GetSettingByUserIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddSettingAsync(CreateSettingRequestDto request, CancellationToken cancellationToken = default);
}