using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.UseCases.Bank.Queries;

public sealed record GetBankByIdQueryRequest(Guid Id) : IQuery<GetBankByIdQueryResponse>;
public sealed record GetBankByIdQueryResponse(Guid Id, string Name, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);

internal sealed class GetBankByIdQueryHandler(IReadUnitOfWork unitOfWork) : IQueryHandler<GetBankByIdQueryRequest, GetBankByIdQueryResponse>
{
    public async Task<Result<GetBankByIdQueryResponse>> Handle(GetBankByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var bankDtoResult = await unitOfWork.Bank.GetBankByIdAsync(new(request.Id), cancellationToken);
        if (bankDtoResult.IsFailure) return Result.Failure<GetBankByIdQueryResponse>(bankDtoResult.Errors);

        var bank = bankDtoResult.Value.Bank;
        if (bank == null) return Result.Success<GetBankByIdQueryResponse>(null);

        return Result.Success(new GetBankByIdQueryResponse(
            Id: bank.Id,
            Name: bank.Name,
            IsActive: bank.IsActive,
            Description: bank.Description,
            ActionedOn: bank.UpdatedOn ?? bank.CreatedOn,
            ActionedBy: bank.UpdatedBy ?? bank.CreatedBy
        ));
    }
}

