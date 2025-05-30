using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Transactions;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class Transaction : WriteEntityBase
{
    private List<BankAccount> _bankAccounts = new List<BankAccount>();
    private List<PeerTransfer> _peerTransfers = new List<PeerTransfer>();
    private List<CurrencyExchange> _currencyExchanges = new List<CurrencyExchange>();
    private List<TransactionItem> _transactionItems = new List<TransactionItem>();

    public decimal Amount { get; private set; }
    public DateTimeOffset TransactedOn { get; protected set; }
    public Guid UserId { get; protected set; }
    public Guid CurrencyId { get; protected set; }
    public bool IsHidden { get; protected set; } = false;

    public User User { get; protected set; } = default!;
    public Currency Currency { get; protected set; } = default!;

    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();
    public IReadOnlyCollection<PeerTransfer> PeerTransfers => _peerTransfers.AsReadOnly();
    public IReadOnlyCollection<CurrencyExchange> CurrencyExchanges => _currencyExchanges.AsReadOnly();
    public IReadOnlyCollection<TransactionItem> TransactionItems => _transactionItems.AsReadOnly();

    private Transaction()
    {
    }

    protected Transaction(decimal amount, DateTimeOffset transactedOn, Guid currencyId, string? description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Amount = amount;
        TransactedOn = transactedOn;
        UserId = actionedBy;
        CurrencyId = currencyId;
    }

    protected static Result<Transaction> Create(decimal amount, DateTimeOffset transactedOn, Guid currencyId, string? description, Guid actionedBy)
    {
        return new Transaction(amount, transactedOn, currencyId, description, actionedBy);
    }

    public Result CreateBankingAccount(decimal amount, decimal? interest,  Guid currencyId, string accountNumber, DateTimeOffset issuedOn, DateTimeOffset closedOn, Guid bankId, string? description, Guid actionedBy)
    {
        if (_bankAccounts.Count > 0)
        {
            return Result.Failure(Errors.Transaction.BankAccountExists);
        }

        var bankingAccountResult = BankAccount.Create(amount, interest, currencyId, accountNumber, description, issuedOn, closedOn, bankId, actionedBy);
        if (bankingAccountResult.IsFailure)
        {
            return Result.Failure(bankingAccountResult.Error);
        }

        IsHidden = true;
        _bankAccounts.Add(bankingAccountResult.Value);

        return Result.Success();
    }

    public Result CreateBankingInterest(decimal? interest, Guid currencyId, string accountNumber, DateTimeOffset issuedOn, DateTimeOffset closedOn, Guid bankId, string? description, Guid actionedBy)
    {
        if (_bankAccounts.Count > 0)
        {
            return Result.Failure(Errors.Transaction.BankAccountExists);
        }

        var bankingAccountResult = BankAccount.Create(amount, interest, currencyId, accountNumber, description, issuedOn, closedOn, bankId, actionedBy);
        if (bankingAccountResult.IsFailure)
        {
            return Result.Failure(bankingAccountResult.Error);
        }

        IsHidden = true;
        _bankAccounts.Add(bankingAccountResult.Value);

        return Result.Success();
    }

    public static Result<Transaction> CreateBankingAccount(BankAccount bankAccount, decimal interest, )
    {
        return BankAccount.Create(amount, currencyId, accountNumber, description, issuedOn, closedOn, bankId, actionedBy);
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

