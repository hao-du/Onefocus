using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Domain.Specifications;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Bank.Commands;

public sealed record CreateBankCommandRequest(string Name, string? Description) : ICommand<CreateBankCommandResponse>;
public sealed record CreateBankCommandResponse(Guid Id);

internal sealed class CreateBankCommandHandler(
    IWriteUnitOfWork writeUnitOfWork,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<CreateBankCommandRequest, CreateBankCommandResponse>(httpContextAccessor)
{
    public override async Task<Result<CreateBankCommandResponse>> Handle(CreateBankCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (validationResult.IsFailure) return Failure(validationResult);

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return Failure(actionByResult);

        var bankCreationResult = Entity.Bank.Create(
            request.Name,
            request.Description,
            actionByResult.Value
        );
        if (bankCreationResult.IsFailure) return Failure(bankCreationResult); ;

        var createResult = await writeUnitOfWork.Bank.AddBankAsync(new(bankCreationResult.Value), cancellationToken);
        if (createResult.IsFailure) return Failure(createResult); ;

        var saveChangesResult = await writeUnitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return Failure(saveChangesResult);

        return Result.Success<CreateBankCommandResponse>(new(bankCreationResult.Value.Id));
    }

    private async Task<Result> ValidateRequest(CreateBankCommandRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return Result.Failure(Errors.Bank.NameRequired);

        var spec = FindNameSpecification<Entity.Bank>.Create(request.Name);
        var queryResult = await writeUnitOfWork.Bank.GetBySpecificationAsync<Entity.Bank>(new(spec), cancellationToken);
        if (queryResult.IsFailure) return queryResult;
        if (queryResult.Value.Entity is not null) return Result.Failure(Errors.Bank.NameIsExisted);

        return Result.Success();
    }
}
