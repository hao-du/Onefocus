using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Contracts.Write.Settings;
using Onefocus.Home.Application.Interfaces.Repositories.Write;
using Onefocus.Home.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Home.Infrastructure.Databases.Repositories.Write;

public sealed class SettingsWriteRepository(
    ILogger<SettingsWriteRepository> logger
        , HomeWriteDbContext context
    ) : BaseContextRepository<SettingsWriteRepository>(logger, context), ISettingsWriteRepository
{
    public async Task<Result<GetSettingsByUserIdResponseDto>> GetSettingsByUserIdAsync(GetSettingsByUserIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var settings = await context.Settings.FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);
            return Result.Success<GetSettingsByUserIdResponseDto>(new(settings));
        });
    }

    public async Task<Result> AddSettingsAsync(CreateSettingsRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddAsync(request.Settings, cancellationToken);
            return Result.Success();
        });
    }
}