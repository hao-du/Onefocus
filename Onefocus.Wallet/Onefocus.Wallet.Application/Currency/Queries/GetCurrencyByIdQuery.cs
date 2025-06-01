using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Read.Currency;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Read;

namespace Onefocus.Wallet.Application.Currency.Queries;

public sealed record GetCurrencyByIdQueryRequest(Guid Id) : IQuery<GetCurrencyByIdQueryResponse>
{
    internal GetCurrencyByIdRequestDto CastToDto() => new(Id);
}

public sealed record GetCurrencyByIdQueryResponse(Guid Id, string Name, string ShortName, bool IsDefault, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy)
{
    public static GetCurrencyByIdQueryResponse? Cast(GetCurrencyByIdResponseDto source)
    {
        if (source == null || source.Currency == null) return null;

        var currencyDto = new GetCurrencyByIdQueryResponse(
            Id: source.Currency.Id,
            Name: source.Currency.Name,
            ShortName: source.Currency.ShortName,
            IsDefault: source.Currency.IsDefault,
            IsActive: source.Currency.IsActive,
            Description: source.Currency.Description,
            ActionedOn: source.Currency.UpdatedOn ?? source.Currency.CreatedOn,
            ActionedBy: source.Currency.UpdatedBy ?? source.Currency.CreatedBy
        );

        return currencyDto;
    }
}


internal sealed class GetCurrencyByIdQueryHandler(IReadUnitOfWork unitOfWork) : IQueryHandler<GetCurrencyByIdQueryRequest, GetCurrencyByIdQueryResponse>
{
    public async Task<Result<GetCurrencyByIdQueryResponse>> Handle(GetCurrencyByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var currencyDtoResult = await unitOfWork.Currency.GetCurrencyByIdAsync(request.CastToDto(), cancellationToken);
        if (currencyDtoResult.IsFailure)
        {
            return Result.Failure<GetCurrencyByIdQueryResponse>(currencyDtoResult.Errors);
        }

        return Result.Success(GetCurrencyByIdQueryResponse.Cast(currencyDtoResult.Value));
    }
}

