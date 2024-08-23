using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write;

public abstract class Transaction : WriteEntityBase
{
    private List<TransactionDetail> _transactionDetails = new List<TransactionDetail>();

    public decimal Amount { get; protected set; }
    public DateTimeOffset TransactedOn { get; protected set; }
    public Guid UserId { get; protected set; }
    public Guid CurrencyId { get; protected set; }
    public IReadOnlyList<TransactionDetail> TransactionDetails => _transactionDetails.AsReadOnly();

    protected Transaction(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, Guid actionedBy)
    {
        Amount = amount;
        TransactedOn = transactedOn;
        CurrencyId = currencyId;
        UserId = userId;

        Init(Guid.NewGuid(), description, actionedBy);
    }

    public Result AddDetail(ObjectValues.TransactionDetail objectValue)
    {
        if(objectValue == null)
        {
            return Result.Failure(CommonErrors.NullReference);
        }

        var detailResult = TransactionDetail.Create(objectValue);
        if (detailResult.IsFailure)
        {
            return detailResult;
        }

        _transactionDetails.Add(detailResult.Value);

        return Result.Success();
    }

    public Result UpdateDetail(ObjectValues.TransactionDetail objectValue)
    {
        if (objectValue == null)
        {
            return Result.Failure(CommonErrors.NullReference);
        }
        if (objectValue.Id == Guid.Empty)
        {
            return Result.Failure(Errors.Transaction.Detail.DetailRequired);
        }

        var detail = _transactionDetails.Find(td => td.Id == objectValue.Id);
        if(detail == null)
        {
            return Result.Failure(CommonErrors.NullReference);
        }

        detail.Update(objectValue);

        return Result.Success();
    }

    public Result MarkDetailAsInactive(Guid detailId, Guid actionedBy)
    {
        if (detailId == Guid.Empty)
        {
            return Result.Failure(Errors.Transaction.Detail.DetailRequired);
        }

        var detail = _transactionDetails.Find(td => td.Id == detailId);
        if (detail == null)
        {
            return Result.Failure(CommonErrors.NullReference);
        }

        detail.MarkInactive(actionedBy);

        return Result.Success();
    }
}

