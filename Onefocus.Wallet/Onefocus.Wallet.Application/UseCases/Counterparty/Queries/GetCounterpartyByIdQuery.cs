using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Wallet.Application.UseCases.Transaction.Queries;

namespace Onefocus.Wallet.Application.UseCases.Counterparty.Queries;

public sealed record GetCounterpartyByIdQueryRequest(Guid Id) : IQuery<GetCounterpartyByIdQueryResponse>;
public sealed record GetCounterpartyByIdQueryResponse(Guid Id, string FullName, string? Email, string? PhoneNumber, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);

internal sealed class GetCounterpartyByIdQueryHandler(
    IHttpContextAccessor httpContextAccessor,
    ILogger<GetAllTransactionsQueryHandler> logger,
    IReadUnitOfWork unitOfWork
) : QueryHandler<GetCounterpartyByIdQueryRequest, GetCounterpartyByIdQueryResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<GetCounterpartyByIdQueryResponse>> Handle(GetCounterpartyByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);
        var userId = getUserIdResult.Value;

        var counterpartyDtoResult = await unitOfWork.Counterparty.GetCounterpartyByIdAsync(new(request.Id, userId), cancellationToken);
        if (counterpartyDtoResult.IsFailure) return counterpartyDtoResult.Failure<GetCounterpartyByIdQueryResponse>();

        var counterparty = counterpartyDtoResult.Value.Counterparty;
        if (counterparty == null) return Result.Success<GetCounterpartyByIdQueryResponse>(null);

        return Result.Success(new GetCounterpartyByIdQueryResponse(
            Id: counterparty.Id,
            FullName: counterparty.FullName,
            Email: counterparty.Email,
            PhoneNumber: counterparty.PhoneNumber,
            IsActive: counterparty.IsActive,
            Description: counterparty.Description,
            ActionedOn: counterparty.UpdatedOn ?? counterparty.CreatedOn,
            ActionedBy: counterparty.UpdatedBy ?? counterparty.CreatedBy
        ));
    }
}

