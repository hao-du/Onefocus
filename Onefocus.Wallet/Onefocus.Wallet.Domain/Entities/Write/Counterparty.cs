using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Interfaces;

namespace Onefocus.Wallet.Domain.Entities.Write;

public sealed class Counterparty : WriteEntityBase, IOwnerUserField, IAggregateRoot
{
    public string FullName { get; private set; } = default!;
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public Guid OwnerUserId { get; private set; }

    public User OwnerUser { get; private set; } = default!;

    private Counterparty()
    {
    }

    private Counterparty(string fullName, string? email, string? phoneNumber, string? description, Guid ownerId, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        OwnerUserId = ownerId;
    }

    public static Result<Counterparty> Create(string fullName, string? email, string? phoneNumber, string? description, Guid ownerId, Guid actionedBy)
    {
        var validationResult = Validate(fullName);
        if (validationResult.IsFailure)
        {
            return Result.Failure<Counterparty>(validationResult.Errors);
        }

        return new Counterparty(fullName, email, phoneNumber, description, ownerId, actionedBy);
    }

    public Result Update(string fullName, string? email, string? phoneNumber, string? description, bool isActive, Guid actionedBy)
    {
        var validationResult = Validate(fullName);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        Description = description;

        if (isActive) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return Result.Success();
    }

    private static Result Validate(string fullName)
    {
        if (string.IsNullOrEmpty(fullName))
        {
            return Result.Failure<Bank>(Errors.Counterparty.FullNameRequired);
        }

        return Result.Success();
    }
}