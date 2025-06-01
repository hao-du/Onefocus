using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Read.Bank;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.Bank.Queries;

public sealed record GetBankByIdQueryRequest(Guid Id) : IQuery<GetBankByIdQueryResponse>
{
    internal GetBankByIdRequestDto CastToDto() => new(Id);
}

public sealed record GetBankByIdQueryResponse(Guid Id, string Name, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy)
{
    public static GetBankByIdQueryResponse? Cast(GetBankByIdResponseDto source)
    {
        if (source == null || source.Bank == null) return null;

        var BankDto = new GetBankByIdQueryResponse(
            Id: source.Bank.Id,
            Name: source.Bank.Name,
            IsActive: source.Bank.IsActive,
            Description: source.Bank.Description,
            ActionedOn: source.Bank.UpdatedOn ?? source.Bank.CreatedOn,
            ActionedBy: source.Bank.UpdatedBy ?? source.Bank.CreatedBy
        );

        return BankDto;
    }
}


internal sealed class GetBankByIdQueryHandler(IReadUnitOfWork unitOfWork) : IQueryHandler<GetBankByIdQueryRequest, GetBankByIdQueryResponse>
{
    public async Task<Result<GetBankByIdQueryResponse>> Handle(GetBankByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var BankDtoResult = await unitOfWork.Bank.GetBankByIdAsync(request.CastToDto(), cancellationToken);
        if (BankDtoResult.IsFailure)
        {
            return Result.Failure<GetBankByIdQueryResponse>(BankDtoResult.Errors);
        }

        return Result.Success(GetBankByIdQueryResponse.Cast(BankDtoResult.Value));
    }
}

