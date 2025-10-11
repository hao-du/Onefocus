using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Wallet.Application.UseCases.Transaction.Queries;

namespace Onefocus.Wallet.Application.UseCases.Bank.Queries;

public sealed record GetBankByIdQueryRequest(Guid Id) : IQuery<GetBankByIdQueryResponse>;
public sealed record GetBankByIdQueryResponse(Guid Id, string Name, bool IsActive, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);

internal sealed class GetBankByIdQueryHandler(
    IHttpContextAccessor httpContextAccessor,
    ILogger<GetAllTransactionsQueryHandler> logger,
    IReadUnitOfWork unitOfWork
) : QueryHandler<GetBankByIdQueryRequest, GetBankByIdQueryResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<GetBankByIdQueryResponse>> Handle(GetBankByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var getUserIdResult = GetUserId();
        if (getUserIdResult.IsFailure) return Failure(getUserIdResult);
        var userId = getUserIdResult.Value;

        var bankDtoResult = await unitOfWork.Bank.GetBankByIdAsync(new(request.Id, userId), cancellationToken);
        if (bankDtoResult.IsFailure) return bankDtoResult.Failure<GetBankByIdQueryResponse>();

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

