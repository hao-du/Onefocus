using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Queries;
public sealed record GetPeerTransferByTransactionIdQueryRequest(Guid TransactionId) : IQuery<GetPeerTransferByTransactionIdQueryResponse>;

public sealed record GetPeerTransferByTransactionIdQueryResponse(
    Guid Id,
    Guid CounterpartyId,
    int Status,
    int Type,
    bool IsActive,
    string? Description,
    IReadOnlyList<GetTransferTransaction> Transactions
);

public sealed record GetTransferTransaction(
    Guid Id,
    DateTimeOffset TransactedOn, 
    Guid CurrencyId, 
    decimal Amount,
    bool IsInFlow,
    string? Description, 
    bool IsActive
);

internal sealed class GetPeerTransferByTransactionIdQueryHandler(
    IReadUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<GetPeerTransferByTransactionIdQueryRequest, GetPeerTransferByTransactionIdQueryResponse>(httpContextAccessor)
{
    public override async Task<Result<GetPeerTransferByTransactionIdQueryResponse>> Handle(GetPeerTransferByTransactionIdQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);
        var userId = getUserIdResult.Value;

        var getPeerTransferResult = await unitOfWork.Transaction.GetPeerTransferByTransactionIdAsync(new(request.TransactionId, userId), cancellationToken);
        if (getPeerTransferResult.IsFailure) return Failure(getPeerTransferResult);

        var peerTransfer = getPeerTransferResult.Value.PeerTransfer;
        if (peerTransfer == null) return Result.Success<GetPeerTransferByTransactionIdQueryResponse>(null);

        var response = new GetPeerTransferByTransactionIdQueryResponse(
            Id: peerTransfer.Id,
            CounterpartyId: peerTransfer.CounterpartyId,
            Status: (int)peerTransfer.Status,
            Type: (int)peerTransfer.Type,
            IsActive: peerTransfer.IsActive,
            Description: peerTransfer.Description,
            Transactions: [..peerTransfer.PeerTransferTransactions.Select(ptt => new GetTransferTransaction(
                Id: ptt.Transaction.Id,
                TransactedOn: ptt.Transaction.TransactedOn,
                CurrencyId: ptt.Transaction.CurrencyId,
                Amount: ptt.Transaction.Amount,
                IsInFlow: ptt.IsInFlow,
                Description: ptt.Transaction.Description,
                IsActive: ptt.Transaction.IsActive
            ))]
        );
        return Result.Success(response);
    }
}
