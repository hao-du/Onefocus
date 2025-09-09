using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.UseCases.Counterparty.Queries;

public sealed record GetAllCounterpartiesQueryRequest() : IQuery<GetAllCounterpartiesQueryResponse>;
public sealed record GetAllCounterpartiesQueryResponse(List<CounterpartyQueryResponse> Counterparties);
public record CounterpartyQueryResponse(Guid Id, string FullName, string? Email, string? PhoneNumber, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);


internal sealed class GetAllCounterpartysQueryHandler(IReadUnitOfWork readUnitOfWork) : IQueryHandler<GetAllCounterpartiesQueryRequest, GetAllCounterpartiesQueryResponse>
{
    public async Task<Result<GetAllCounterpartiesQueryResponse>> Handle(GetAllCounterpartiesQueryRequest request, CancellationToken cancellationToken)
    {
        var counterpartyDtosResult = await readUnitOfWork.Counterparty.GetAllCounterpartysAsync(cancellationToken);
        if (counterpartyDtosResult.IsFailure) return counterpartyDtosResult.Failure<GetAllCounterpartiesQueryResponse>();
        var counterpartyDtos = counterpartyDtosResult.Value.Counterparties;
        return Result.Success(new GetAllCounterpartiesQueryResponse(
            Counterparties: [.. counterpartyDtos.Select(c => new CounterpartyQueryResponse(
                Id: c.Id,
                FullName: c.FullName,
                Email: c.Email,
                PhoneNumber: c.PhoneNumber,
                IsActive: c.IsActive,
                Description: c.Description,
                ActionedOn: c.UpdatedOn ?? c.CreatedOn,
                ActionedBy: c.UpdatedBy ?? c.UpdatedBy
            ))]
        ));
    }
}

