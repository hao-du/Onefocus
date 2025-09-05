using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Domain.Specifications;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Counterparty.Commands;

public sealed record UpdateCounterpartyCommandRequest(Guid Id, string FullName, string? Email, string? PhoneNumber, bool IsActive, string? Description) : ICommand;

internal sealed class UpdateCounterpartyCommandHandler(
    IWriteUnitOfWork writeUnitOfWork,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<UpdateCounterpartyCommandRequest>(httpContextAccessor)
{
    public override async Task<Result> Handle(UpdateCounterpartyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request, cancellationToken);
        if (validationResult.IsFailure) return validationResult;

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult;

        var getCounterpartyResult = await writeUnitOfWork.Counterparty.GetCounterpartyByIdAsync(new(request.Id), cancellationToken);
        if (getCounterpartyResult.IsFailure) return getCounterpartyResult;
        if (getCounterpartyResult.Value.Counterparty == null) return Result.Failure(CommonErrors.NullReference);

        var updateResult = getCounterpartyResult.Value.Counterparty.Update(
            request.FullName,
            request.Email,
            request.PhoneNumber,
            request.Description,
            request.IsActive,
            actionByResult.Value
        );
        if (updateResult.IsFailure) return updateResult;

        var saveChangesResult = await writeUnitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return saveChangesResult;

        return Result.Success();
    }

    private Result ValidateRequest(UpdateCounterpartyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = Entity.Counterparty.Validate(request.FullName);
        return validationResult;
    }
}