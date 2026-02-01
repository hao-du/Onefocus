using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Bank.Commands;

public sealed record UpdateBankCommandRequest(Guid Id, string Name, bool IsActive, string? Description) : ICommand;

internal sealed class UpdateBankCommandHandler(
    IBankService bankService,
    IDomainEventService domainEventService,
    IWriteUnitOfWork writeUnitOfWork,
    ILogger<UpdateBankCommandHandler> logger,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<UpdateBankCommandRequest>(httpContextAccessor, logger)
{
    public override async Task<Result> Handle(UpdateBankCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (validationResult.IsFailure) return validationResult;

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult;

        var getBankResult = await writeUnitOfWork.Bank.GetBankByIdAsync(new(request.Id), cancellationToken);
        if (getBankResult.IsFailure) return getBankResult;

        var bank = getBankResult.Value.Bank;

        if (bank == null) return Result.Failure(CommonErrors.NullReference);

        var updateResult = bank.Update(request.Name, request.Description, request.IsActive, actionByResult.Value);
        if (updateResult.IsFailure) return updateResult;

        if (bank.DomainEvents.Count > 0)
        {
            var addSearchIndexEventResult = await domainEventService.AddSearchIndexEvent(bank.DomainEvents, cancellationToken);
            if (addSearchIndexEventResult.IsFailure) return addSearchIndexEventResult;
            bank.ClearDomainEvents();
        }

        var saveChangesResult = await writeUnitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return saveChangesResult;

        return Result.Success();
    }

    private async Task<Result> ValidateRequest(UpdateBankCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = Entity.Bank.Validate(request.Name);
        if (validationResult.IsFailure) return validationResult;

        var checkDuplicationResult = await bankService.HasDuplicatedBank(request.Id, request.Name, cancellationToken);
        if (checkDuplicationResult.IsFailure) return checkDuplicationResult;

        return Result.Success();
    }
}