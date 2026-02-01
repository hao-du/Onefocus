using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
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
    ILogger<UpdateCurrencyExchangeCommandHandler> logger,
    IDomainEventService domainEventService,
    IWriteUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<UpdateCurrencyExchangeCommandRequest>(httpContextAccessor, logger)
{
    public override async Task<Result> Handle(UpdateCurrencyExchangeCommandRequest request, CancellationToken cancellationToken)
    {
        var getSourceCurrencyResult = await unitOfWork.Currency.GetCurrencyByIdAsync(new(request.SourceCurrencyId), cancellationToken);
        if (getSourceCurrencyResult.IsFailure) { return getSourceCurrencyResult; }
        var sourceCurrency = getSourceCurrencyResult.Value.Currency;

        var getTargetCurrencyResult = await unitOfWork.Currency.GetCurrencyByIdAsync(new(request.TargetCurrencyId), cancellationToken);
        if (getTargetCurrencyResult.IsFailure) { return getTargetCurrencyResult; }
        var targetCurrency = getTargetCurrencyResult.Value.Currency;

        var validationResult = ValidateRequest(request, sourceCurrency, targetCurrency);
        if (validationResult.IsFailure) return validationResult;

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult;

        var getCurrencyExchangeResult = await unitOfWork.Transaction.GetCurrencyExchangeByIdAsync(new(request.Id), cancellationToken);
        if (getCurrencyExchangeResult.IsFailure) return getCurrencyExchangeResult;

        var currencyExchange = getCurrencyExchangeResult.Value.CurrencyExchange;
        if (currencyExchange == null) return Result.Failure(CommonErrors.NullReference);

        var updateCashflowResult = currencyExchange.Update(
            source: CurrencyExchangeParams.Create(request.SourceAmount, sourceCurrency),
            target: CurrencyExchangeParams.Create(request.TargetAmount, targetCurrency),
            exchangeRate: request.ExchangeRate,
            transactedOn: request.TransactedOn,
            description: request.Description,
            isActive: request.IsActive,
            actionedBy: actionByResult.Value
        );
        if (updateCashflowResult.IsFailure) return updateCashflowResult;

        if (currencyExchange.DomainEvents.Count > 0)
        {
            var addSearchIndexEventResult = await domainEventService.AddSearchIndexEvent(currencyExchange.DomainEvents, cancellationToken);
            if (addSearchIndexEventResult.IsFailure) return addSearchIndexEventResult;
            currencyExchange.ClearDomainEvents();
        }

        var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return saveChangesResult;

        return Result.Success();
    }

    private static Result ValidateRequest(UpdateCurrencyExchangeCommandRequest request, Entity.Currency? sourceCurrency, Entity.Currency? targetCurrency)
    {
        return Entity.TransactionTypes.CurrencyExchange.Validate(
            source: CurrencyExchangeParams.Create(request.SourceAmount, sourceCurrency),
            target: CurrencyExchangeParams.Create(request.TargetAmount, targetCurrency),
            exchangeRate: request.ExchangeRate
        );
    }
}

