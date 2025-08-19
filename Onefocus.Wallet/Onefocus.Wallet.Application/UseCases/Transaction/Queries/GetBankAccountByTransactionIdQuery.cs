using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Queries;
public sealed record GetBankAccountByTransactionIdQueryRequest(Guid TransactionId) : IQuery<GetBankAccountByTransactionIdQueryResponse>;

public sealed record GetBankAccountByTransactionIdQueryResponse(
    Guid Id,
    decimal Amount,
    Guid CurrencyId,
    decimal InterestRate,
    string AccountNumber,
    DateTimeOffset IssuedOn,
    DateTimeOffset? ClosedOn, 
    bool IsClosed,
    Guid BankId,
    string? Description,
    IReadOnlyList<GetTransaction> Transactions
);

public sealed record GetTransaction(
    Guid Id,
    DateTimeOffset TransactedOn, 
    Guid CurrencyId, 
    decimal Amount, 
    string? Description, 
    bool IsActive
);

internal sealed class GetBankAccountByTransactionIdQueryHandler(
    IReadUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<GetBankAccountByTransactionIdQueryRequest, GetBankAccountByTransactionIdQueryResponse>(httpContextAccessor)
{
    public override async Task<Result<GetBankAccountByTransactionIdQueryResponse>> Handle(GetBankAccountByTransactionIdQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);
        var userId = getUserIdResult.Value;

        var getBankAccountResult = await unitOfWork.Transaction.GetBankAccountByTransactionIdAsync(new(request.TransactionId, userId), cancellationToken);
        if (getBankAccountResult.IsFailure) return Failure(getBankAccountResult);

        var bankAccount = getBankAccountResult.Value.BankAccount;
        if (bankAccount == null) return Result.Success<GetBankAccountByTransactionIdQueryResponse>(null);

        var response = new GetBankAccountByTransactionIdQueryResponse(
            Id: bankAccount.Id,
            Amount: bankAccount.Amount,
            CurrencyId: bankAccount.CurrencyId,
            InterestRate: bankAccount.InterestRate,
            AccountNumber: bankAccount.AccountNumber,
            IssuedOn: bankAccount.IssuedOn,
            ClosedOn: bankAccount.ClosedOn,
            IsClosed: bankAccount.IsClosed,
            BankId: bankAccount.BankId,
            Description: bankAccount.Description,
            Transactions: [..bankAccount.BankAccountTransactions.Select(bct => new GetTransaction(
                Id: bct.Transaction.Id,
                TransactedOn: bct.Transaction.TransactedOn,
                CurrencyId: bct.Transaction.CurrencyId,
                Amount: bct.Transaction.Amount,
                Description: bct.Transaction.Description,
                IsActive: bct.Transaction.IsActive
            ))]
        );
        return Result.Success(response);
    }
}
