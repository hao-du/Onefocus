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
public sealed record CreateCurrencyCommandRequest(string Name, string ShortName, bool DefaultFlag, string? Description) : ICommand;

internal sealed class CreateCurrencyCommandHandler : ICommandHandler<CreateCurrencyCommandRequest>
{
    private readonly IReadUnitOfWork _readUnitOfWork;
    private readonly IWriteUnitOfWork _writeUnitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateCurrencyCommandHandler(
        IReadUnitOfWork readUnitOfWork
        , IWriteUnitOfWork writeUnitOfWork
        , IHttpContextAccessor httpContextAccessor
    )
    {
        _readUnitOfWork = readUnitOfWork;
        _writeUnitOfWork = writeUnitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(CreateCurrencyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (validationResult.IsFailure) return Result.Failure(validationResult.Error);

        if (!Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid actionedBy))
        {
            return Result.Failure(CommonErrors.UserClaimInvalid);
        }

        var addCurrencyResult = Currency.Create(
               name: request.Name,
               shortName: request.ShortName,
               description: request.Description,
               defaultFlag: request.DefaultFlag,
               actionedBy: actionedBy
            );
        if(addCurrencyResult.IsFailure) return Result.Failure(addCurrencyResult.Error);

        var repoResult = await _writeUnitOfWork.Currency.AddCurrencyAsync(new(addCurrencyResult.Value), cancellationToken);
        if (repoResult.IsFailure) return Result.Failure(repoResult.Error);

        var transactionResult = await _writeUnitOfWork.WithTransactionAsync(async (cancellationToken) =>
        {
            if (request.DefaultFlag)
            {
                var bulkUpdateResult = await _writeUnitOfWork.Currency.BulkMarkDefaultFlag(new(new List<Guid>(), true, false, actionedBy));
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

