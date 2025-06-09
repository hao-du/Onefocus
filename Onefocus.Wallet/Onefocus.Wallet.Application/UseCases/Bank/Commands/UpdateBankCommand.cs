using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Domain.Specifications;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Bank.Commands;

public sealed record UpdateBankCommandRequest(Guid Id, string Name, bool IsActive, string? Description) : ICommand;

internal sealed class UpdateBankCommandHandler(
    IWriteUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<UpdateBankCommandRequest>(httpContextAccessor)
{
    public override async Task<Result> Handle(UpdateBankCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (validationResult.IsFailure) return validationResult;

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult;

        var getBankResult = await unitOfWork.Bank.GetBankByIdAsync(new(request.Id), cancellationToken);
        if (getBankResult.IsFailure) return getBankResult;
        if (getBankResult.Value.Bank == null) return Result.Failure(CommonErrors.NullReference);

        var updateResult = getBankResult.Value.Bank.Update(request.Name, request.Description, request.IsActive, actionByResult.Value);
        if (updateResult.IsFailure) return updateResult;

        var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return saveChangesResult;

        return Result.Success();
    }

    private async Task<Result> ValidateRequest(UpdateBankCommandRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return Result.Failure(Errors.Bank.NameRequired);

        var spec = FindNameSpecification<Entity.Bank>.Create(request.Name).And(ExcludeIdsSpecification<Entity.Bank>.Create([request.Id]));
        var queryResult = await unitOfWork.Bank.GetBySpecificationAsync<Entity.Bank>(new(spec), cancellationToken);
        if (queryResult.IsFailure) return queryResult;
        if (queryResult.Value.Entity is not null) return Result.Failure(Errors.Bank.NameIsExisted);

        return Result.Success();
    }
}