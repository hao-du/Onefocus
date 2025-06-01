using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Read.Bank;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Read;
using static Onefocus.Wallet.Application.Bank.Queries.GetAllBanksQueryResponse;

namespace Onefocus.Wallet.Application.Bank.Queries;

public sealed record GetAllBanksQueryRequest() : IQuery<GetAllBanksQueryResponse>;

public sealed record GetAllBanksQueryResponse(List<BankQueryResponse> Banks)
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0305:Simplify collection initialization", Justification = "ToList() is more readable.")]
    public static GetAllBanksQueryResponse Cast(GetAllBanksResponseDto source)
    {
        var currencyResponses = new GetAllBanksQueryResponse(
            Banks: source.Banks.Select(c => new BankQueryResponse(
                Id: c.Id,
                Name: c.Name,
                IsActive: c.IsActive,
                Description: c.Description,
                ActionedOn: c.UpdatedOn ?? c.CreatedOn,
                ActionedBy: c.UpdatedBy ?? c.UpdatedBy
            )).ToList()
        );

        return currencyResponses;
    }

    public record BankQueryResponse(Guid Id, string Name, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);
}


internal sealed class GetAllBanksQueryHandler(IReadUnitOfWork readUnitOfWork) : IQueryHandler<GetAllBanksQueryRequest, GetAllBanksQueryResponse>
{
    public async Task<Result<GetAllBanksQueryResponse>> Handle(GetAllBanksQueryRequest request, CancellationToken cancellationToken)
    {
        var bankDtosResult = await readUnitOfWork.Bank.GetAllBanksAsync(cancellationToken);
        if (bankDtosResult.IsFailure)
        {
            return Result.Failure<GetAllBanksQueryResponse>(bankDtosResult.Errors);
        }

        return Result.Success<GetAllBanksQueryResponse>(Cast(bankDtosResult.Value));
    }
}

