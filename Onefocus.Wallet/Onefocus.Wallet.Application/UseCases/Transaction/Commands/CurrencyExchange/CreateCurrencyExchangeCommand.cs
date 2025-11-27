using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Application.UseCases.Transaction.Commands.CashFlow;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Commands.CurrencyExchange;
public sealed record CreateCurrencyExchangeCommandRequest(
    decimal SourceAmount, 
    Guid SourceCurrencyId,
    decimal TargetAmount,
    Guid TargetCurrencyId,
    decimal ExchangeRate,
    DateTimeOffset TransactedOn, 
    string? Description
) : ICommand<CreateCurrencyExchangeCommandResponse>;
public sealed record CreateCurrencyExchangeCommandResponse(Guid Id);

internal sealed class CreateCurrencyExchangeCommandHandler(
    ITransactionService transactionService,
    ILogger<CreateCurrencyExchangeCommandHandler> logger,
    IWriteUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<CreateCurrencyExchangeCommandRequest, CreateCurrencyExchangeCommandResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<CreateCurrencyExchangeCommandResponse>> Handle(CreateCurrencyExchangeCommandRequest request, CancellationToken cancellationToken)
    {
        var getSourceCurrencyResult = await unitOfWork.Currency.GetCurrencyByIdAsync(new(request.SourceCurrencyId), cancellationToken);
        if (getSourceCurrencyResult.IsFailure) { return Failure(getSourceCurrencyResult); }
        var sourceCurrency = getSourceCurrencyResult.Value.Currency;

        var getTargetCurrencyResult = await unitOfWork.Currency.GetCurrencyByIdAsync(new(request.TargetCurrencyId), cancellationToken);
        if (getTargetCurrencyResult.IsFailure) { return Failure(getTargetCurrencyResult); }
        var targetCurrency = getTargetCurrencyResult.Value.Currency;

        var validationResult = ValidateRequest(request, sourceCurrency, targetCurrency);
        if (validationResult.IsFailure) return Failure(validationResult);

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return Failure(actionByResult);

        var addCurrencyExchangeResult = Entity.TransactionTypes.CurrencyExchange.Create(
            source: CurrencyExchangeParams.Create(request.SourceAmount, sourceCurrency),
            target: CurrencyExchangeParams.Create(request.TargetAmount, targetCurrency),
            exchangeRate: request.ExchangeRate,
            transactedOn: request.TransactedOn,
            description: request.Description,
            ownerId: actionByResult.Value,
            actionedBy: actionByResult.Value
        );
        if (addCurrencyExchangeResult.IsFailure) return Failure(addCurrencyExchangeResult);

        var currencyExchange = addCurrencyExchangeResult.Value;
        var repoResult = await unitOfWork.Transaction.AddCurrencyExchangeAsync(new(currencyExchange), cancellationToken);
        if (repoResult.IsFailure) return Failure(repoResult);

        var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return Failure(saveChangesResult);

        await transactionService.PublishEvents(currencyExchange, cancellationToken);

        return Result.Success<CreateCurrencyExchangeCommandResponse>(new(currencyExchange.Id));
    }

    private static Result ValidateRequest(CreateCurrencyExchangeCommandRequest request, Entity.Currency? sourceCurrency, Entity.Currency? targetCurrency)
    {
        return Entity.TransactionTypes.CurrencyExchange.Validate(
            source: CurrencyExchangeParams.Create(request.SourceAmount, sourceCurrency),
            target: CurrencyExchangeParams.Create(request.TargetAmount, targetCurrency),
            exchangeRate: request.ExchangeRate
        );
    }
}

