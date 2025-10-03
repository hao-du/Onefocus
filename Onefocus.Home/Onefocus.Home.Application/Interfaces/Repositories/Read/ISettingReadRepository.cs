using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Contracts.Read.Setting;

namespace Onefocus.Home.Application.Interfaces.Repositories.Read;

public interface ISettingReadRepository : IBaseContextRepository
{
    Task<Result<GetSettingByUserIdResponseDto>> GetSettingByUserIdAsync(GetSettingByUserIdRequestDto request, CancellationToken cancellationToken = default);
}