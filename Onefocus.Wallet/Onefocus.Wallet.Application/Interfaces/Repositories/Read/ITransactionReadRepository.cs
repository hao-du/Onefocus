using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.Transaction;
using Onefocus.Wallet.Application.Contracts.Read.Transaction.BankAccount;
using Onefocus.Wallet.Application.Contracts.Read.Transaction.CashFlow;
using Onefocus.Wallet.Application.Contracts.Read.Transaction.CurrencyExchange;
using Onefocus.Wallet.Application.Contracts.Read.Transaction.PeerTransfer;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Read;

public interface ITransactionReadRepository : IBaseContextRepository
{
    Task<Result<GetAllTransactionsResponseDto>> GetAllTransactionsAsync(GetAllTransactionsRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<GetCashFlowByTransactionIdResponseDto>> GetCashFlowByTransactionIdAsync(GetCashFlowByTransactionIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<GetBankAccountByTransactionIdResponseDto>> GetBankAccountByTransactionIdAsync(GetBankAccountByTransactionIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<GetPeerTransferByTransactionIdResponseDto>> GetPeerTransferByTransactionIdAsync(GetPeerTransferByTransactionIdRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<GetCurrencyExchangeByTransactionIdResponseDto>> GetCurrencyExchangeByTransactionIdAsync(GetCurrencyExchangeByTransactionIdRequestDto request, CancellationToken cancellationToken = default);
}