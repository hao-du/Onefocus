using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Write.Bank;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Wallet.Infrastructure.Repositories.Write;

public sealed class BankWriteRepository(
    ILogger<BankWriteRepository> logger
        , WalletWriteDbContext context
    ) : BaseContextRepository<BankWriteRepository>(logger, context), IBankWriteRepository
{
    public async Task<Result<GetBankByIdResponseDto>> GetBankByIdAsync(GetBankByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var bank = await context.Bank.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            return Result.Success<GetBankByIdResponseDto>(new(bank));
        });
    }

    public async Task<Result> AddBankAsync(CreateBankRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddAsync(request.Bank, cancellationToken);
            return Result.Success();
        });
    }
}