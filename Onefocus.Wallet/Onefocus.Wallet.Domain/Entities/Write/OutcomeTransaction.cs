using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class TransferTransaction : Transaction
{
    public Guid TransferredUserID { get; private set; }
    public User? TransferredUser { get; private set; }

    private TransferTransaction(decimal amount, DateTimeOffset traceDate, Guid userId, Guid transferredUserID, Guid currencyId, string description, Guid actionedBy): base(amount, traceDate, userId, currencyId, description, actionedBy)
    {
        TransferredUserID= transferredUserID;
    }

    public static Result<TransferTransaction> Create(decimal amount, DateTimeOffset traceDate, Guid userId, Guid transferredUserID, Guid currencyId, string description, Guid actionedBy)
    {
        if(amount < 0)
        {
            return Result.Failure<TransferTransaction>(Errors.Transaction.AmountMustGreaterThanZero);
        }
        if (userId == Guid.Empty)
        {
            return Result.Failure<TransferTransaction>(Errors.User.UserRequired);
        }
        if (transferredUserID == Guid.Empty)
        {
            return Result.Failure<TransferTransaction>(Errors.Transaction.Transfer.TransferredUserRequired);
        }
        if (currencyId == Guid.Empty)
        {
            return Result.Failure<TransferTransaction>(Errors.Currency.CurrencyRequired);
        }

        return new TransferTransaction(amount, traceDate, userId, transferredUserID, currencyId, description, actionedBy);
    }

    public Result<TransferTransaction> Update(decimal amount, DateTimeOffset traceDate, Guid userId, Guid transferredUserID, Guid currencyId, string description, bool activeFlag, Guid actionedBy)
    {
        if (amount < 0)
        {
            return Result.Failure<TransferTransaction>(Errors.Transaction.AmountMustGreaterThanZero);
        }
        if (userId == Guid.Empty)
        {
            return Result.Failure<TransferTransaction>(Errors.User.UserRequired);
        }
        if (transferredUserID == Guid.Empty)
        {
            return Result.Failure<TransferTransaction>(Errors.Transaction.Transfer.TransferredUserRequired);
        }
        if (currencyId == Guid.Empty)
        {
            return Result.Failure<TransferTransaction>(Errors.Currency.CurrencyRequired);
        }

        Amount = amount;
        TraceDate = traceDate;
        UserId = userId;
        TransferredUserID = transferredUserID;
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

