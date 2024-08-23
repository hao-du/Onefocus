using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public class IncomeTransaction : Transaction
{
    private IncomeTransaction(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, Guid actionedBy) : base(amount, transactedOn, userId, currencyId, description, actionedBy)
    {
    }

    public static Result<IncomeTransaction> Create(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, Guid actionedBy)
    {
        var validationResult = Validate(amount, transactedOn, userId, currencyId);
        if (validationResult.IsFailure)
        {
            return Result.Failure<IncomeTransaction>(validationResult.Error);
        }

        return new IncomeTransaction(amount, transactedOn, userId, currencyId, description, actionedBy);
    }

    public Result Update(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, bool activeFlag, Guid actionedBy)
    {
        var validationResult = Validate(amount, transactedOn, userId, currencyId);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Amount = amount;
        TransactedOn = transactedOn;
        UserId = userId;
        CurrencyId = currencyId;
        Description = description;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return Result.Success();
    }

    private static Result Validate(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId)
    {
        if (amount < 0)
        {
            return Result.Failure(Errors.Transaction.AmountMustGreaterThanZero);
        }
        if (userId == Guid.Empty)
        {
            return Result.Failure(Errors.User.UserRequired);
        }
        if (currencyId == Guid.Empty)
        {
            return Result.Failure(Errors.Currency.CurrencyRequired);
        }

        return Result.Success();
    }
}

