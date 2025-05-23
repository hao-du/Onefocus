﻿using Microsoft.AspNetCore.Http;
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
public sealed record UpdateCurrencyCommandRequest(Guid Id, string Name, string ShortName, bool DefaultFlag, bool ActiveFlag, string? Description) : ICommand;

internal sealed class UpdateCurrencyCommandHandler : CommandHandler<UpdateCurrencyCommandRequest>
{
    private readonly IReadUnitOfWork _readUnitOfWork;
    private readonly IWriteUnitOfWork _writeUnitOfWork;

    public UpdateCurrencyCommandHandler(
        IReadUnitOfWork readUnitOfWork
        , IWriteUnitOfWork writeUnitOfWork
        , IHttpContextAccessor httpContextAccessor
    )
    : base(httpContextAccessor)
    {
        _readUnitOfWork = readUnitOfWork;
        _writeUnitOfWork = writeUnitOfWork;
    }

    public override async Task<Result> Handle(UpdateCurrencyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (validationResult.IsFailure) return Result.Failure(validationResult.Error);

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return Result.Failure(actionByResult.Error);

        var currencyResult = await _writeUnitOfWork.Currency.GetCurrencyByIdAsync(new(request.Id));
        if (currencyResult.IsFailure) return Result.Failure(currencyResult.Error);
        if (currencyResult.Value.Currency == null) return Result.Failure(CommonErrors.NullReference);

        var updateResult = currencyResult.Value.Currency.Update(
            name: request.Name,
            shortName: request.ShortName,
            description: request.Description,
            defaultFlag: request.DefaultFlag,
            activeFlag: request.ActiveFlag,
            actionedBy: actionByResult.Value
        );

        if (updateResult.IsFailure) return Result.Failure(updateResult.Error);

        var transactionResult = await _writeUnitOfWork.WithTransactionAsync(async (cancellationToken) =>
        {
            if (request.DefaultFlag)
            {
                var bulkUpdateResult = await _writeUnitOfWork.Currency.BulkMarkDefaultFlag(new(new List<Guid>(), true, false, actionByResult.Value));
                if (bulkUpdateResult.IsFailure)
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

    private async Task<Result> ValidateRequest(UpdateCurrencyCommandRequest request, CancellationToken cancellationToken)
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

