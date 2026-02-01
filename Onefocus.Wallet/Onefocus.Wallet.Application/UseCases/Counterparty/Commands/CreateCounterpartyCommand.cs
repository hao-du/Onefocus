using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Counterparty.Commands;

public sealed record CreateCounterpartyCommandRequest(string FullName, string? Email, string? PhoneNumber, string? Description) : ICommand<CreateCounterpartyCommandResponse>;
public sealed record CreateCounterpartyCommandResponse(Guid Id);

internal sealed class CreateCounterpartyCommandHandler(
    IWriteUnitOfWork writeUnitOfWork,
    IDomainEventService domainEventService,
    ILogger<CreateCounterpartyCommandHandler> logger,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<CreateCounterpartyCommandRequest, CreateCounterpartyCommandResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<CreateCounterpartyCommandResponse>> Handle(CreateCounterpartyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return Failure(validationResult);

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return Failure(actionByResult);

        var counterpartyCreationResult = Entity.Counterparty.Create(
            request.FullName,
            request.Email,
            request.PhoneNumber,
            request.Description,
            ownerId: actionByResult.Value,
            actionByResult.Value
        );
        if (counterpartyCreationResult.IsFailure) return Failure(counterpartyCreationResult);

        var counterparty = counterpartyCreationResult.Value;

        var createResult = await writeUnitOfWork.Counterparty.AddCounterpartyAsync(new(counterparty), cancellationToken);
        if (createResult.IsFailure) return Failure(createResult);

        if (counterparty.DomainEvents.Count > 0)
        {
            var addSearchIndexEventResult = await domainEventService.AddSearchIndexEvent(counterparty.DomainEvents, cancellationToken);
            if (addSearchIndexEventResult.IsFailure) return Failure(addSearchIndexEventResult);
            counterparty.ClearDomainEvents();
        }

        var saveChangesResult = await writeUnitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return Failure(saveChangesResult);

        return Result.Success<CreateCounterpartyCommandResponse>(new(counterparty.Id));
    }

    private static Result ValidateRequest(CreateCounterpartyCommandRequest request)
    {
        var validationResult = Entity.Counterparty.Validate(request.FullName);
        return validationResult;
    }
}
