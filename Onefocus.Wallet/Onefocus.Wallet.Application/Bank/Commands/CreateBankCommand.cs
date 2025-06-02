using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Domain.Specifications;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Messages.Write.Bank;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Write;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Bank.Commands;

public sealed record CreateBankCommandRequest(string Name, string? Description) : ICommand;

internal sealed class CreateBankCommandHandler(
    IWriteUnitOfWork writeUnitOfWork,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<CreateBankCommandRequest>(httpContextAccessor)
{
    public override async Task<Result> Handle(CreateBankCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (validationResult.IsFailure) return validationResult;

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult;

        var bankCreationResult = Entity.Bank.Create(
            request.Name,
            request.Description,
            actionByResult.Value
        );
        if (bankCreationResult.IsFailure) return bankCreationResult;

        var createResult = await writeUnitOfWork.Bank.AddBankAsync(new CreateBankRequestDto(bankCreationResult.Value), cancellationToken);
        if (createResult.IsFailure) return createResult;

        await writeUnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private async Task<Result> ValidateRequest(CreateBankCommandRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return Result.Failure(Errors.Bank.NameRequired);

        var spec = FindNameSpecification<Entity.Bank>.Create(request.Name);
        var queryResult = await writeUnitOfWork.Bank.GetBySpecificationAsync<Entity.Bank>(new(spec), cancellationToken);
        if (queryResult.IsFailure) return queryResult;
        if (queryResult.Value.Entity != null) return Result.Failure(Errors.Bank.NameIsExisted);

        return Result.Success();
    }
}
