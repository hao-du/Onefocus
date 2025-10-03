using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Contracts.Read.Setting;
using Onefocus.Home.Application.Interfaces.Repositories.Read;
using Onefocus.Home.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Home.Infrastructure.Repositories.Read;

public sealed class SettingReadRepository(
    ILogger<SettingReadRepository> logger
        , HomeReadDbContext context
    ) : BaseContextRepository<SettingReadRepository>(logger, context), ISettingReadRepository
{
    public async Task<Result<GetSettingByUserIdResponseDto>> GetSettingByUserIdAsync(GetSettingByUserIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var setting = await context.Setting.FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);
            return Result.Success<GetSettingByUserIdResponseDto>(new(setting));
        });
    }
}