﻿using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Transactions;

namespace Onefocus.Wallet.Domain.Entities.Write;

public sealed class Currency : WriteEntityBase
{
    private List<Transaction> _transactions = new List<Transaction>();
    private List<ExchangeTransaction> _exchangeTransactions = new List<ExchangeTransaction>();

    public string Name { get; private set; }
    public string ShortName { get; private set; }
    public bool DefaultFlag { get; private set; }

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<ExchangeTransaction> ExchangeTransactions => _exchangeTransactions.AsReadOnly();

    private Currency()
    {
        Name = default!;
        ShortName = default!;
    }

    private Currency(string name, string shortName, string? description, bool defaultFlag, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Name = name;
        ShortName = shortName;
        DefaultFlag = defaultFlag;
    }

    public static Result<Currency> Create(string name, string shortName, string? description, bool defaultFlag, Guid actionedBy)
    {
        var validationResult = Validate(name, shortName);
        if (validationResult.IsFailure)
        {
            return Result.Failure<Currency>(validationResult.Error);
        }

        return new Currency(name, shortName, description, defaultFlag, actionedBy);
    }

    public Result Update(string name, string shortName, string? description, bool defaultFlag, bool activeFlag, Guid actionedBy)
    {
        var validationResult = Validate(name, shortName);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Name = name;
        ShortName = shortName.ToUpper();
        Description = description;
        DefaultFlag = defaultFlag;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return Result.Success();
    }

    public void MarkDefault(bool defaultFlag, Guid actionedBy)
    {
        DefaultFlag = defaultFlag;
        Update(actionedBy);
    }

    private static Result Validate(string name, string shortName)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<Currency>(Errors.Currency.NameRequired);
        }
        if (string.IsNullOrEmpty(shortName))
        {
            return Result.Failure<Currency>(Errors.Currency.ShortNameRequired);
        }
        if (shortName.Length < 3 || shortName.Length > 4)
        {
            return Result.Failure<Currency>(Errors.Currency.ShortNameLengthMustBeThreeOrFour);
        }

        return Result.Success();
    }
}