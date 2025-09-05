using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Write.Counterparty;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Wallet.Infrastructure.Repositories.Write;

public sealed class CounterpartyWriteRepository(
    ILogger<CounterpartyWriteRepository> logger
        , WalletWriteDbContext context
    ) : BaseContextRepository<CounterpartyWriteRepository>(logger, context), ICounterpartyWriteRepository
{
    public async Task<Result<GetCounterpartyByIdResponseDto>> GetCounterpartyByIdAsync(GetCounterpartyByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var counterparty = await context.Counterparty.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            return Result.Success<GetCounterpartyByIdResponseDto>(new(counterparty));
        });
    }

    public async Task<Result> AddCounterpartyAsync(CreateCounterpartyRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddAsync(request.Counterparty, cancellationToken);
            return Result.Success();
        });
    }
}