using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Bank.Commands;

public sealed record CreateBankCommandRequest(string Name, string? Description) : ICommand<CreateBankCommandResponse>;
public sealed record CreateBankCommandResponse(Guid Id);

internal sealed class CreateBankCommandHandler(
    IBankService bankService,
    IWriteUnitOfWork writeUnitOfWork,
    ILogger<CreateBankCommandHandler> logger,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<CreateBankCommandRequest, CreateBankCommandResponse>(httpContextAccessor, logger)
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
            ownerId: actionByResult.Value,
            actionByResult.Value
        );
        if (bankCreationResult.IsFailure) return Failure(bankCreationResult);

        var bank = bankCreationResult.Value;

        var createResult = await writeUnitOfWork.Bank.AddBankAsync(new(bank), cancellationToken);
        if (createResult.IsFailure) return Failure(createResult); ;

        var saveChangesResult = await writeUnitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return Failure(saveChangesResult);

        await bankService.PublishEvents(bank, cancellationToken);

        return Result.Success<CreateBankCommandResponse>(new(bankCreationResult.Value.Id));
    }

    private async Task<Result> ValidateRequest(CreateBankCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = Entity.Bank.Validate(request.Name);
        if (validationResult.IsFailure) return validationResult;

        var checkDuplicationResult = await bankService.HasDuplicatedBank(Guid.Empty, request.Name, cancellationToken);
        if (checkDuplicationResult.IsFailure) return checkDuplicationResult;

        return Result.Success();
    }
}
