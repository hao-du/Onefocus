using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.BankAccount;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.CashFlow;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.CurrencyExchange;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.PeerTransfer;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Write;

public interface ITransactionWriteRepository : IBaseContextRepository
{
    Task<Result<GetCashFlowByIdResponseDto>> GetCashFlowByIdAsync(GetCashFlowByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<GetBankAccountByIdResponseDto>> GetBankAccountByIdAsync(GetBankAccountByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<GetPeerTransferByIdResponseDto>> GetPeerTransferByIdAsync(GetPeerTransferByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<GetCurrencyExchangeByIdResponseDto>> GetCurrencyExchangeByIdAsync(GetCurrencyExchangeByIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddCashFlowAsync(CreateCashFlowRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddBankAccountAsync(CreateBankAccountRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddPeerTransferAsync(CreatePeerTransferRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddCurrencyExchangeAsync(CreateCurrencyExchangeRequestDto request, CancellationToken cancellationToken = default);
}