using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Wallet.Application.UseCases.Transaction.Queries;

namespace Onefocus.Wallet.Application.UseCases.Bank.Queries;

public sealed record GetBanksQueryRequest(string? Name, string? Description) : IQuery<GetBanksQueryResponse>;
public sealed record GetBanksQueryResponse(List<BankQueryResponse> Banks);
public record BankQueryResponse(Guid Id, string Name, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);


internal sealed class GetBanksQueryHandler(
    IHttpContextAccessor httpContextAccessor,
    ILogger<GetAllTransactionsQueryHandler> logger,
    IReadUnitOfWork readUnitOfWork
) : QueryHandler<GetBanksQueryRequest, GetBanksQueryResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<GetBanksQueryResponse>> Handle(GetBanksQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);
        var userId = getUserIdResult.Value;

        var bankDtosResult = await readUnitOfWork.Bank.GetBanksAsync(new(userId, request.Name, request.Description), cancellationToken);
        if (bankDtosResult.IsFailure) return bankDtosResult.Failure<GetBanksQueryResponse>();
        var bankDtos = bankDtosResult.Value.Banks;
        return Result.Success(new GetBanksQueryResponse(
            Banks: [.. bankDtos.Select(c => new BankQueryResponse(
                Id: c.Id,
                Name: c.Name,
                IsActive: c.IsActive,
                Description: c.Description,
                ActionedOn: c.UpdatedOn ?? c.CreatedOn,
                ActionedBy: c.UpdatedBy ?? c.UpdatedBy
            ))]
        ));
    }
}

