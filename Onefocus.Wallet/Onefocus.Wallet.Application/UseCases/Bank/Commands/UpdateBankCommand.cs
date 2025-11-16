using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Domain.Specifications;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.Contracts.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Events.Bank;
using Onefocus.Wallet.Domain.Events.Counterparty;
using static MassTransit.Monitoring.Performance.BuiltInCounters;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Bank.Commands;

public sealed record UpdateBankCommandRequest(Guid Id, string Name, bool IsActive, string? Description) : ICommand;

internal sealed class UpdateBankCommandHandler(
    IBankService bankService,
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

        var saveChangesResult = await writeUnitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return saveChangesResult;

        await bankService.PublishEvents(bank, cancellationToken);

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