using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Entities.Write;
using Onefocus.Wallet.Domain.Messages.Write;
using Onefocus.Wallet.Domain.Specifications.Currency;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Read;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Write;
using System.Security.Claims;

namespace Onefocus.Membership.Application.User.Commands;
public sealed record CreateCurrencyCommandRequest(string Name, string ShortName, bool IsDefault, string? Description) : ICommand;

internal sealed class CreateCurrencyCommandHandler : CommandHandler<CreateCurrencyCommandRequest>
{
    private readonly IReadUnitOfWork _readUnitOfWork;
    private readonly IWriteUnitOfWork _writeUnitOfWork;

    public CreateCurrencyCommandHandler(
        IReadUnitOfWork readUnitOfWork
        , IWriteUnitOfWork writeUnitOfWork
        , IHttpContextAccessor httpContextAccessor
    )
    : base( httpContextAccessor )
    {
        _readUnitOfWork = readUnitOfWork;
        _writeUnitOfWork = writeUnitOfWork;
    }

    public async Task<Result> Handle(CreateCurrencyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (validationResult.IsFailure) return Result.Failure(validationResult.Error);

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return Result.Failure(actionByResult.Error);

        var addCurrencyResult = Currency.Create(
               name: request.Name,
               shortName: request.ShortName,
               description: request.Description,
               isDefault: request.IsDefault,
               actionedBy: actionByResult.Value
            );
        if(addCurrencyResult.IsFailure) return Result.Failure(addCurrencyResult.Error);

        var repoResult = await _writeUnitOfWork.Currency.AddCurrencyAsync(new(addCurrencyResult.Value), cancellationToken);
        if (repoResult.IsFailure) return Result.Failure(repoResult.Error);

        var transactionResult = await _writeUnitOfWork.WithTransactionAsync(async (cancellationToken) =>
        {
            if (request.IsDefault)
            {
                var bulkUpdateResult = await _writeUnitOfWork.Currency.BulkMarkDefaultFlag(new(new List<Guid>(), true, false, actionByResult.Value));
                if(bulkUpdateResult.IsFailure)
                {
                    return Result.Failure(bulkUpdateResult.Error);
                }
            }

            await _writeUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }, cancellationToken);
        if (transactionResult.IsFailure)
        {
            return Result.Failure(transactionResult.Error);
        } 

        return Result.Success();
    }

    private async Task<Result> ValidateRequest(CreateCurrencyCommandRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Name)) return Result.Failure(Errors.Currency.NameRequired);
        if (string.IsNullOrEmpty(request.ShortName)) return Result.Failure(Errors.Currency.ShortNameRequired);
        if (request.ShortName.Length < 3 || request.ShortName.Length > 4) return Result.Failure(Errors.Currency.ShortNameLengthMustBeThreeOrFour);

        var spec = FindNameSpecification.Create(request.Name).Or(FindShortNameSpecification.Create(request.ShortName));
        var queryResult = await _readUnitOfWork.Currency.GetCurrencyBySpecificationAsync(new(spec), cancellationToken);
        if (queryResult.IsFailure)
        {
            return Result.Failure(queryResult.Error);
        }
        if (queryResult.Value.Currencies.Count > 0)
        {
            return Result.Failure(Errors.Currency.NameOrShortNameIsExisted);
        }

        return Result.Success();
    }
}

