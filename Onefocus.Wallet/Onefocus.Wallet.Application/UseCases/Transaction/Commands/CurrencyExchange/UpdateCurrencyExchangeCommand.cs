using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Commands.CurrencyExchange;
public sealed record UpdateCurrencyExchangeCommandRequest(
    Guid Id, 
    decimal SourceAmount,
    Guid SourceCurrencyId,
    decimal TargetAmount,
    Guid TargetCurrencyId,
    decimal ExchangeRate,
    DateTimeOffset TransactedOn,
    bool IsActive,
    string? Description
) : ICommand;

internal sealed class UpdateCurrencyExchangeCommandHandler(
    ILogger<UpdateCurrencyExchangeCommandHandler> logger
        , IWriteUnitOfWork unitOfWork
        , IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<UpdateCurrencyExchangeCommandRequest>(httpContextAccessor, logger)
{
    public override async Task<Result> Handle(UpdateCurrencyExchangeCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return validationResult;

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult;

        var getCurrencyExchangeResult = await unitOfWork.Transaction.GetCurrencyExchangeByIdAsync(new(request.Id), cancellationToken);
        if (getCurrencyExchangeResult.IsFailure) return getCurrencyExchangeResult;

        var currencyExchange = getCurrencyExchangeResult.Value.CurrencyExchange;
        if (currencyExchange == null) return Result.Failure(CommonErrors.NullReference);

        var updateCashflowResult = currencyExchange.Update(
            source: CurrencyExchangeParams.Create(request.SourceAmount, request.SourceCurrencyId),
            target: CurrencyExchangeParams.Create(request.TargetAmount, request.TargetCurrencyId),
            exchangeRate: request.ExchangeRate,
            transactedOn: request.TransactedOn,
            description: request.Description,
            isActive: request.IsActive,
            actionedBy: actionByResult.Value
        );
        if (updateCashflowResult.IsFailure) return updateCashflowResult;

        var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return saveChangesResult;

        return Result.Success();
    }

    private static Result ValidateRequest(UpdateCurrencyExchangeCommandRequest request)
    {
        return Entity.TransactionTypes.CurrencyExchange.Validate(
            source: CurrencyExchangeParams.Create(request.SourceAmount, request.SourceCurrencyId),
            target: CurrencyExchangeParams.Create(request.TargetAmount, request.TargetCurrencyId),
            exchangeRate: request.ExchangeRate
        );
    }
}

