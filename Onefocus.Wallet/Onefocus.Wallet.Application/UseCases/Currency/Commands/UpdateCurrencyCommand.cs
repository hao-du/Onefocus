using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Currency.Commands;
public sealed record UpdateCurrencyCommandRequest(Guid Id, string Name, string ShortName, bool IsDefault, bool IsActive, string? Description) : ICommand;

internal sealed class UpdateCurrencyCommandHandler(
        ICurrencyService currencyService
        , IWriteUnitOfWork unitOfWork
        , IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<UpdateCurrencyCommandRequest>(httpContextAccessor)
{
    public override async Task<Result> Handle(UpdateCurrencyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (validationResult.IsFailure) return validationResult;

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult;

        var currencyResult = await unitOfWork.Currency.GetCurrencyByIdAsync(new(request.Id), cancellationToken);
        if (currencyResult.IsFailure) return currencyResult;
        if (currencyResult.Value.Currency == null) return Result.Failure(CommonErrors.NullReference);

        var updateResult = currencyResult.Value.Currency.Update(
            name: request.Name,
            shortName: request.ShortName,
            description: request.Description,
            isDefault: request.IsDefault,
            isActive: request.IsActive,
            actionedBy: actionByResult.Value
        );
        if (updateResult.IsFailure) return updateResult;

        var transactionResult = await unitOfWork.WithTransactionAsync(async (cancellationToken) =>
        {
            if (request.IsDefault)
            {
                var bulkUpdateResult = await unitOfWork.Currency.BulkMarkDefaultFlag(new([], true, false, actionByResult.Value), cancellationToken);
                if (bulkUpdateResult.IsFailure) return bulkUpdateResult.Error;
            }

            var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
            return saveChangesResult;
        }, cancellationToken);

        return transactionResult;
    }

    private async Task<Result> ValidateRequest(UpdateCurrencyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = Entity.Currency.Validate(request.Name, request.ShortName);
        if (validationResult.IsFailure) return validationResult;

        var checkDuplicationResult = await currencyService.HasDuplicatedCurrency(request.Id, request.Name, request.ShortName, cancellationToken);
        if (checkDuplicationResult.IsFailure) return checkDuplicationResult;

        return Result.Success();
    }
}

