using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.UseCases.Bank.Queries;

public sealed record GetAllBanksQueryRequest() : IQuery<GetAllBanksQueryResponse>;
public sealed record GetAllBanksQueryResponse(List<BankQueryResponse> Banks);
public record BankQueryResponse(Guid Id, string Name, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);


internal sealed class GetAllBanksQueryHandler(IReadUnitOfWork readUnitOfWork) : IQueryHandler<GetAllBanksQueryRequest, GetAllBanksQueryResponse>
{
    public async Task<Result<GetAllBanksQueryResponse>> Handle(GetAllBanksQueryRequest request, CancellationToken cancellationToken)
    {
        var bankDtosResult = await readUnitOfWork.Bank.GetAllBanksAsync(cancellationToken);
        if (bankDtosResult.IsFailure) return bankDtosResult.Failure<GetAllBanksQueryResponse>();
        var bankDtos = bankDtosResult.Value.Banks;
        return Result.Success(new GetAllBanksQueryResponse(
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

