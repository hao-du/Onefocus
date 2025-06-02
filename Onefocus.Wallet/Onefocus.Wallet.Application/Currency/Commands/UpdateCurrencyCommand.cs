using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Domain.Specification;
using Onefocus.Common.Abstractions.Domain.Specifications;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Specifications.Write.Currency;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Write;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Currency.Commands;
public sealed record UpdateCurrencyCommandRequest(Guid Id, string Name, string ShortName, bool IsDefault, bool IsActive, string? Description) : ICommand;

internal sealed class UpdateCurrencyCommandHandler(
        IWriteUnitOfWork unitOfWork
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

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }, cancellationToken);

        return transactionResult;
    }

    private async Task<Result> ValidateRequest(UpdateCurrencyCommandRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Name)) return Result.Failure(Errors.Currency.NameRequired);
        if (string.IsNullOrEmpty(request.ShortName)) return Result.Failure(Errors.Currency.ShortNameRequired);
        if (request.ShortName.Length < 3 || request.ShortName.Length > 4) return Result.Failure(Errors.Currency.ShortNameLengthMustBeThreeOrFour);

        var orSpec = new OrSpecification<Entity.Currency>(
            FindNameSpecification<Entity.Currency>.Create(request.Name),
            FindShortNameSpecification.Create(request.ShortName)
        );
        var spec = orSpec.And(ExcludeIdsSpecification<Entity.Currency>.Create([request.Id]));

        var queryResult = await unitOfWork.Currency.GetBySpecificationAsync<Entity.Currency>(new(spec), cancellationToken);
        if (queryResult.IsFailure) return queryResult;
        if (queryResult.Value != null) return Result.Failure(Errors.Currency.NameOrShortNameIsExisted);

        return Result.Success();
    }
}

