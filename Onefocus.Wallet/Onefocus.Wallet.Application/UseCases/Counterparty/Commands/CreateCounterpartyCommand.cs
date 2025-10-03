using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Counterparty.Commands;

public sealed record CreateCounterpartyCommandRequest(string FullName, string? Email, string? PhoneNumber, string? Description) : ICommand<CreateCounterpartyCommandResponse>;
public sealed record CreateCounterpartyCommandResponse(Guid Id);

internal sealed class CreateCounterpartyCommandHandler(
    ILogger<CreateCounterpartyCommandHandler> logger,
    IWriteUnitOfWork writeUnitOfWork,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<CreateCounterpartyCommandRequest, CreateCounterpartyCommandResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<CreateCounterpartyCommandResponse>> Handle(CreateCounterpartyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request, cancellationToken);
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
        if (counterpartyCreationResult.IsFailure) return Failure(counterpartyCreationResult); ;

        var createResult = await writeUnitOfWork.Counterparty.AddCounterpartyAsync(new(counterpartyCreationResult.Value), cancellationToken);
        if (createResult.IsFailure) return Failure(createResult); ;

        var saveChangesResult = await writeUnitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return Failure(saveChangesResult);

        return Result.Success<CreateCounterpartyCommandResponse>(new(counterpartyCreationResult.Value.Id));
    }

    private Result ValidateRequest(CreateCounterpartyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = Entity.Counterparty.Validate(request.FullName);
        return validationResult;
    }
}
