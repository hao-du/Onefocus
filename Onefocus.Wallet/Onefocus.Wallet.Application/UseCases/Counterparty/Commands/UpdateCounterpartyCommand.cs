using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Counterparty.Commands;

public sealed record UpdateCounterpartyCommandRequest(Guid Id, string FullName, string? Email, string? PhoneNumber, bool IsActive, string? Description) : ICommand;

internal sealed class UpdateCounterpartyCommandHandler(
    IWriteUnitOfWork writeUnitOfWork,
    IDomainEventService domainEventService,
    ILogger<UpdateCounterpartyCommandHandler> logger,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<UpdateCounterpartyCommandRequest>(httpContextAccessor, logger)
{
    public override async Task<Result> Handle(UpdateCounterpartyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return validationResult;

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult;

        var getCounterpartyResult = await writeUnitOfWork.Counterparty.GetCounterpartyByIdAsync(new(request.Id), cancellationToken);
        if (getCounterpartyResult.IsFailure) return getCounterpartyResult;

        var counterparty = getCounterpartyResult.Value.Counterparty;

        if (counterparty == null) return Result.Failure(CommonErrors.NullReference);

        var updateResult = counterparty.Update(
            request.FullName,
            request.Email,
            request.PhoneNumber,
            request.Description,
            request.IsActive,
            actionByResult.Value
        );
        if (updateResult.IsFailure) return updateResult;

        if (counterparty.DomainEvents.Count > 0)
        {
            var addSearchIndexEventResult = await domainEventService.AddSearchIndexEvent(counterparty.DomainEvents, cancellationToken);
            if (addSearchIndexEventResult.IsFailure) return addSearchIndexEventResult;
            counterparty.ClearDomainEvents();
        }

        var saveChangesResult = await writeUnitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return saveChangesResult;

        return Result.Success();
    }

    private static Result ValidateRequest(UpdateCounterpartyCommandRequest request)
    {
        var validationResult = Entity.Counterparty.Validate(request.FullName);
        return validationResult;
    }
}