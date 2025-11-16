using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Domain.Specifications;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Specifications.Write.Currency;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Currency.Commands;
public sealed record CreateCurrencyCommandRequest(string Name, string ShortName, bool IsDefault, string? Description) : ICommand<CreateBankCommandResponse>;

public sealed record CreateBankCommandResponse(Guid Id);

internal sealed class CreateCurrencyCommandHandler(
        ILogger<CreateCurrencyCommandHandler> logger
        , ICurrencyService currencyService
        , IWriteUnitOfWork unitOfWork
        , IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<CreateCurrencyCommandRequest, CreateBankCommandResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<CreateBankCommandResponse>> Handle(CreateCurrencyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (validationResult.IsFailure) return Failure(validationResult);

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return Failure(actionByResult);

        var addCurrencyResult = Entity.Currency.Create(
               name: request.Name,
               shortName: request.ShortName,
               description: request.Description,
               isDefault: request.IsDefault,
               ownerId: actionByResult.Value,
               actionedBy: actionByResult.Value
            );
        if (addCurrencyResult.IsFailure) return Failure(addCurrencyResult);

        var currency = addCurrencyResult.Value;

        var repoResult = await unitOfWork.Currency.AddCurrencyAsync(new(currency), cancellationToken);
        if (repoResult.IsFailure) return Failure(repoResult);

        var transactionResult = await unitOfWork.WithTransactionAsync(async (cancellationToken) =>
        {
            if (request.IsDefault)
            {
                var bulkUpdateResult = await unitOfWork.Currency.BulkMarkDefaultFlag(new([], true, false, actionByResult.Value), cancellationToken);
                if (bulkUpdateResult.IsFailure) return Failure(bulkUpdateResult);
            }

            var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
            if (saveChangesResult.IsFailure) return Failure(saveChangesResult);

            return Result.Success<CreateBankCommandResponse>(new(currency.Id));
        }, cancellationToken);
        if(transactionResult.IsFailure) return transactionResult;

        await currencyService.PublishEvents(currency, cancellationToken);

        return transactionResult;
    }

    private async Task<Result> ValidateRequest(CreateCurrencyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = Entity.Currency.Validate(request.Name, request.ShortName);
        if (validationResult.IsFailure) return validationResult;

        var checkDuplicationResult = await currencyService.HasDuplicatedCurrency(Guid.Empty, request.Name, request.ShortName, cancellationToken);
        if (checkDuplicationResult.IsFailure) return checkDuplicationResult;

        return Result.Success();
    }
}

