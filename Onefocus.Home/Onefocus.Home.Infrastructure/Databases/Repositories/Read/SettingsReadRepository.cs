using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Contracts.Read.Settings;
using Onefocus.Home.Application.Interfaces.Repositories.Read;
using Onefocus.Home.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Home.Infrastructure.Databases.Repositories.Read;

public sealed class SettingsReadRepository(
    ILogger<SettingsReadRepository> logger
        , HomeReadDbContext context
    ) : BaseContextRepository<SettingsReadRepository>(logger, context), ISettingsReadRepository
{
    public async Task<Result<GetSettingsByUserIdResponseDto>> GetSettingsByUserIdAsync(GetSettingsByUserIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var settings = await context.Settings.FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);
            return Result.Success<GetSettingsByUserIdResponseDto>(new(settings));
        });
    }
}