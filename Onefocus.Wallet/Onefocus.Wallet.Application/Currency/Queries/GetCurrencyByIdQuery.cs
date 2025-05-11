using MediatR;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Wallet.Domain.Messages.Read.Currency;
using Onefocus.Wallet.Domain.Repositories.Read;
using static Onefocus.Membership.Application.User.Commands.GetAllCurrenciesQueryResponse;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record GetCurrencyByIdQueryRequest(Guid Id) : IQuery<GetCurrencyByIdQueryResponse>
{
    internal GetCurrencyByIdRequestDto CastToDto() => new(Id);
}

public sealed record GetCurrencyByIdQueryResponse(Guid Id, string Name, string ShortName, bool DefaultFlag, bool ActiveFlag, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy)
{
    public static GetCurrencyByIdQueryResponse? Cast(GetCurrencyByIdResponseDto source)
    {
        if (source == null || source.Currency == null) return null;

        var currencyDto = new GetCurrencyByIdQueryResponse (
            Id: source.Currency.Id,
            Name: source.Currency.Name,
            ShortName: source.Currency.ShortName,
            DefaultFlag: source.Currency.DefaultFlag,
            ActiveFlag: source.Currency.ActiveFlag,
            Description: source.Currency.Description,
            ActionedOn: source.Currency.UpdatedOn ?? source.Currency.CreatedOn,
            ActionedBy: source.Currency.UpdatedBy ?? source.Currency.CreatedBy
        );

        return currencyDto;
    }
}


internal sealed class GetCurrencyByIdQueryHandler : IQueryHandler<GetCurrencyByIdQueryRequest, GetCurrencyByIdQueryResponse>
{
    private readonly ICurrencyReadRepository _currencyRepository;

    public GetCurrencyByIdQueryHandler(ICurrencyReadRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<Result<GetCurrencyByIdQueryResponse>> Handle(GetCurrencyByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var currencyDtoResult = await _currencyRepository.GetCurrencyByIdAsync(request.CastToDto(), cancellationToken);
        if (currencyDtoResult.IsFailure)
        {
            return Result.Failure<GetCurrencyByIdQueryResponse>(currencyDtoResult.Error);
        }

        return Result.Success<GetCurrencyByIdQueryResponse>(GetCurrencyByIdQueryResponse.Cast(currencyDtoResult.Value));
    }
}

