using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.Counterparty;
using Onefocus.Wallet.Application.Interfaces.Repositories.Read;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Wallet.Infrastructure.Repositories.Read;

public sealed class CounterpartyReadRepository(
    ILogger<CounterpartyReadRepository> logger
        , WalletReadDbContext context
    ) : BaseContextRepository<CounterpartyReadRepository>(logger, context), ICounterpartyReadRepository
{
    public async Task<Result<GetAllCounterpartiesResponseDto>> GetAllCounterpartysAsync(GetAllCounterpartiesRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var counterparties = await context.Counterparty.Where(c => c.OwnerUserId == request.UserId).ToListAsync(cancellationToken);
            return Result.Success<GetAllCounterpartiesResponseDto>(new(counterparties));
        });
    }

    public async Task<Result<GetCounterpartyByIdResponseDto>> GetCounterpartyByIdAsync(GetCounterpartyByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var counterparty = await context.Counterparty.FirstOrDefaultAsync(c => c.Id == request.Id && c.OwnerUserId == request.UserId, cancellationToken);
            return Result.Success<GetCounterpartyByIdResponseDto>(new(counterparty));
        });
    }
}