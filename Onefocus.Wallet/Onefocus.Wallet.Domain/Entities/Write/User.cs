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

    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string Email { get; private set; } = default!;

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();
    public IReadOnlyCollection<Bank> Banks => _banks.AsReadOnly();
    public IReadOnlyCollection<Counterparty> Counterparties => _counterparties.AsReadOnly();
    public IReadOnlyCollection<Currency> Currencies => _currencies.AsReadOnly();

    private User()
    {
        // Required for EF Core
    }

    private User(Guid? id, string email, string firstName, string lastName, string? description, Guid actionedBy)
    {
        Init(description, actionedBy);

        Id = id ?? Guid.CreateVersion7();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Result<User> Create(Guid? id, string email, string firstName, string lastName, string? description, Guid actionedBy)
    {
        var validationResult = Validate(email, firstName, lastName);
        if (validationResult.IsFailure) return (Result<User>)validationResult;

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

        SetActiveFlag(isActive, actionedBy);

        return this;
    }
    private static Result Validate(string email, string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure(Errors.User.EmailRequired);
        }
        if (string.IsNullOrEmpty(firstName))
        {
            return Result.Failure(Errors.User.FirstNameRequired);
        }
        if (string.IsNullOrEmpty(lastName))
        {
            return Result.Failure(Errors.User.LastNameRequired);
        }

        return Result.Success();
    }
}