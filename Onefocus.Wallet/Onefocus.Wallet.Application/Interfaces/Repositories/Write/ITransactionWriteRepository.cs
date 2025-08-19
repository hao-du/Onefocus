using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.BankAccount;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.CashFlow;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Write;

public interface ITransactionWriteRepository : IBaseContextRepository
{
    Task<Result<GetCashFlowByIdResponseDto>> GetCashFlowByTransactionIdAsync(GetCashFlowByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<GetBankAccountByIdResponseDto>> GetBankAccountByTransactionIdAsync(GetBankAccountByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddCashFlowAsync(CreateCashFlowRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddBankAccountAsync(CreateBankAccountRequestDto request, CancellationToken cancellationToken = default);
}