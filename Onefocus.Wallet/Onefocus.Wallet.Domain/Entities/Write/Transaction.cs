using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Models;
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

    protected Result AddDetail(TransactionDetailParams detailParams)
    {
        if(detailParams == null)
        {
            return Result.Failure(CommonErrors.NullReference);
        }
        if (detailParams.Id != Guid.Empty)
        {
            return Result.Failure(Errors.Transaction.Detail.DetailMustBeNew);
        }

        var detailResult = TransactionDetail.Create(detailParams);
        if (detailResult.IsFailure)
        {
            return detailResult;
        }

        _transactionDetails.Add(detailResult.Value);

        return Result.Success();
    }

    protected Result UpsertDetail(TransactionDetailParams detailParams)
    {
        if (detailParams == null)
        {
            return Result.Failure(CommonErrors.NullReference);
        }
        if (detailParams.Id == Guid.Empty)
        {
            return AddDetail(detailParams);
        }

        var detail = _transactionDetails.Find(td => td.Id == detailParams.Id);
        if(detail == null)
        {
            return Result.Failure(CommonErrors.NullReference);
        }

        detail.Update(detailParams);

        return Result.Success();
    }
}

