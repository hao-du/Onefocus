using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Onefocus.Wallet.Domain.Entities.Write;

public abstract class Transaction : WriteEntityBase
{
    private List<TransactionDetail> _transactionDetails = new List<TransactionDetail>();

    public DateTimeOffset TransactedOn { get; protected set; }
    public Guid UserId { get; protected set; }
    public Guid CurrencyId { get; protected set; }

    public User User { get; protected set; } = default!;
    public Currency Currency { get; protected set; } = default!;
    public IReadOnlyCollection<TransactionDetail> TransactionDetails => _transactionDetails.AsReadOnly();

    protected Transaction()
    {
    }

    protected Transaction(DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, Guid actionedBy)
    {
        TransactedOn = transactedOn;
        CurrencyId = currencyId;
        UserId = userId;

        Init(Guid.NewGuid(), description, actionedBy);
    }

    protected Result AddDetail(ObjectValues.TransactionDetail objectValue)
    {
        if(objectValue == null)
        {
            return Result.Failure(CommonErrors.NullReference);
        }
        if (objectValue.Id != Guid.Empty)
        {
            return Result.Failure(Errors.Transaction.Detail.DetailMustBeNew);
        }

        var detailResult = TransactionDetail.Create(objectValue);
        if (detailResult.IsFailure)
        {
            return detailResult;
        }

        _transactionDetails.Add(detailResult.Value);

        return Result.Success();
    }

    protected Result UpsertDetail(ObjectValues.TransactionDetail objectValue)
    {
        if (objectValue == null)
        {
            return Result.Failure(CommonErrors.NullReference);
        }
        if (objectValue.Id == Guid.Empty)
        {
            return AddDetail(objectValue);
        }

        var detail = _transactionDetails.Find(td => td.Id == objectValue.Id);
        if(detail == null)
        {
            return Result.Failure(CommonErrors.NullReference);
        }

        detail.Update(objectValue);

        return Result.Success();
    }
}

