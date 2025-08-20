using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Queries;
public sealed record GetPeerTransferByTransactionIdQueryRequest(Guid TransactionId) : IQuery<GetPeerTransferByTransactionIdQueryResponse>;

public sealed record GetPeerTransferByTransactionIdQueryResponse(
    Guid Id,
    Guid CounterpartyId,
    int PeerTransferStatus,
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
            PeerTransferStatus: (int)peerTransfer.Status,
            Type: (int)peerTransfer.Type,
            IsActive: peerTransfer.IsActive,
            Description: peerTransfer.Description,
            Transactions: [..peerTransfer.PeerTransferTransactions.Select(bct => new GetTransferTransaction(
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
