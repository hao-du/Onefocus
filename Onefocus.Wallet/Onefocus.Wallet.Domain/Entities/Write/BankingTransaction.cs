using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class BankingTransaction : Transaction
{
    public Guid BankId { get; private set; }
    public Bank Bank { get; private set; }

    private BankingTransaction(decimal amount, DateTimeOffset traceDate, Guid userId, Guid currencyId, string description, Guid actionedBy): base(amount, traceDate, userId, currencyId, description, actionedBy)
    {
    }

    public static Result<BankingTransaction> Create(decimal amount, DateTimeOffset traceDate, Guid userId, Guid currencyId, string description, Guid actionedBy)
    {
        if(amount < 0)
        {
            return Result.Failure<BankingTransaction>(Errors.Transaction.AmountMustGreaterThanZero);
        }
        if (userId == Guid.Empty)
        {
            return Result.Failure<BankingTransaction>(Errors.User.UserRequired);
        }
        if (currencyId == Guid.Empty)
        {
            return Result.Failure<BankingTransaction>(Errors.Currency.CurrencyRequired);
        }

        return new BankingTransaction(amount, traceDate, userId, currencyId, description, actionedBy);
    }

    public Result<BankingTransaction> Update(decimal amount, DateTimeOffset traceDate, Guid userId, Guid currencyId, string description, bool activeFlag, Guid actionedBy)
    {
        if (amount < 0)
        {
            return Result.Failure<BankingTransaction>(Errors.Transaction.AmountMustGreaterThanZero);
        }
        if (userId == Guid.Empty)
        {
            return Result.Failure<BankingTransaction>(Errors.User.UserRequired);
        }
        if (currencyId == Guid.Empty)
        {
            return Result.Failure<BankingTransaction>(Errors.Currency.CurrencyRequired);
        }

        Amount = amount;
        TraceDate = traceDate;
        UserId = userId;
        CurrencyId = currencyId;
        Description = description;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return this;
    }

    public void SetActiveFlag(bool activeFlag, Guid actionedBy)
    {
        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);
    }
}

