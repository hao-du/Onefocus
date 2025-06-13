using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Domain.Specifications;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Specifications.Write.Currency;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Currency.Commands;
public sealed record CreateCurrencyCommandRequest(string Name, string ShortName, bool IsDefault, string? Description) : ICommand<CreateBankCommandResponse>;

public sealed record CreateBankCommandResponse(Guid Id);

internal sealed class CreateCurrencyCommandHandler(
        IWriteUnitOfWork unitOfWork
        , IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<CreateCurrencyCommandRequest, CreateBankCommandResponse>(httpContextAccessor)
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

        var repoResult = await unitOfWork.Currency.AddCurrencyAsync(new(addCurrencyResult.Value), cancellationToken);
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

            return Result.Success<CreateBankCommandResponse>(new(addCurrencyResult.Value.Id));
        }, cancellationToken);

        return transactionResult;
    }

    private async Task<Result> ValidateRequest(CreateCurrencyCommandRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Name)) return Result.Failure(Errors.Currency.NameRequired);
        if (string.IsNullOrEmpty(request.ShortName)) return Result.Failure(Errors.Currency.ShortNameRequired);
        if (request.ShortName.Length < 3 || request.ShortName.Length > 4) return Result.Failure(Errors.Currency.ShortNameLengthMustBeThreeOrFour);

        var spec = FindNameSpecification<Entity.Currency>.Create(request.Name).Or(FindShortNameSpecification.Create(request.ShortName));
        var queryResult = await unitOfWork.Currency.GetBySpecificationAsync<Entity.Currency>(new(spec), cancellationToken);
        if (queryResult.IsFailure) return queryResult;
        if (queryResult.Value.Entity != null) return Result.Failure(Errors.Currency.NameOrShortNameIsExisted);

        return Result.Success();
    }
}

