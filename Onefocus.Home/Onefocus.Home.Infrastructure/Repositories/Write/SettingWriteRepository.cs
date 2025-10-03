using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Contracts.Write.Setting;
using Onefocus.Home.Application.Interfaces.Repositories.Write;
using Onefocus.Home.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Home.Infrastructure.Repositories.Write;

public sealed class SettingWriteRepository(
    ILogger<SettingWriteRepository> logger
        , HomeWriteDbContext context
    ) : BaseContextRepository<SettingWriteRepository>(logger, context), ISettingWriteRepository
{
    public async Task<Result<GetSettingByUserIdResponseDto>> GetSettingByUserIdAsync(GetSettingByUserIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var setting = await context.Setting.FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);
            return Result.Success<GetSettingByUserIdResponseDto>(new(setting));
        });
    }

    public async Task<Result> AddSettingAsync(CreateSettingRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddAsync(request.Setting, cancellationToken);
            return Result.Success();
        });
    }
}