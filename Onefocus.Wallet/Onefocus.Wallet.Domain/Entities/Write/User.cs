using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Write;

public sealed class User : WriteEntityBase, IAggregateRoot
{
    private readonly List<Transaction> _transactions = [];
    private readonly List<BankAccount> _bankAccounts = [];
    private readonly List<Bank> _banks = [];
    private readonly List<Counterparty> _counterparties = [];
    private readonly List<Currency> _currencies = [];
    private readonly List<Option> _options = [];

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();
    public IReadOnlyCollection<Bank> Banks => _banks.AsReadOnly();
    public IReadOnlyCollection<Counterparty> Counterparties => _counterparties.AsReadOnly();
    public IReadOnlyCollection<Currency> Currencies => _currencies.AsReadOnly();
    public IReadOnlyCollection<Option> Options => _options.AsReadOnly();

    private User()
    {
        FirstName = default!;
        LastName = default!;
        Email = default!;
    }

    private User(Guid? id, string email, string firstName, string lastName, string? description, Guid actionedBy)
    {
        Init(id ?? Guid.NewGuid(), description, actionedBy);

        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Result<User> Create(Guid? id, string email, string firstName, string lastName, string? description, Guid actionedBy)
    {
        var validationResult = Validate(email, firstName, lastName);
        if (validationResult.IsFailure)
        {
            return Result.Failure<User>(validationResult.Errors);
        }

        return new User(id, email, firstName, lastName, description, actionedBy);
    }

    public Result<User> Update(string email, string firstName, string lastName, string? description, bool isActive, Guid actionedBy)
    {
        var validationResult = Validate(email, firstName, lastName);
        if (validationResult.IsFailure)
        {
            return Result.Failure<User>(validationResult.Errors);
        }

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Description = description;

        if (isActive) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return this;
    }
    private static Result Validate(string email, string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<User>(Errors.User.EmailRequired);
        }
        if (string.IsNullOrEmpty(firstName))
        {
            return Result.Failure<User>(Errors.User.FirstNameRequired);
        }
        if (string.IsNullOrEmpty(lastName))
        {
            return Result.Failure<User>(Errors.User.LastNameRequired);
        }

        return Result.Success();
    }
}