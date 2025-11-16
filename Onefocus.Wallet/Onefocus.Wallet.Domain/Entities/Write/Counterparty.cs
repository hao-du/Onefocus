using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Interfaces;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;
using Onefocus.Wallet.Domain.Events.Bank;
using Onefocus.Wallet.Domain.Events.Counterparty;

namespace Onefocus.Wallet.Domain.Entities.Write;

public sealed class Counterparty : WriteEntityBase, IOwnerUserField, IAggregateRoot
{
    private readonly List<PeerTransfer> _peerTransfers = [];

    public string FullName { get; private set; } = default!;
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public Guid OwnerUserId { get; private set; }

    public User OwnerUser { get; private set; } = default!;

    public IReadOnlyCollection<PeerTransfer> PeerTransfers => _peerTransfers.AsReadOnly();

    private Counterparty()
    {
        // Required for EF Core
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
        if (validationResult.IsFailure) return (Result<Counterparty>)validationResult;

        var counterparty = new Counterparty(fullName, email, phoneNumber, description, ownerId, actionedBy);

        counterparty.AddDomainEvent(CounterpartyUpsertedEvent.Create(counterparty));

        return counterparty;
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

        SetActiveFlag(isActive, actionedBy);

        AddDomainEvent(CounterpartyUpsertedEvent.Create(this));

        return Result.Success();
    }

    public static Result Validate(string fullName)
    {
        if (string.IsNullOrEmpty(fullName))
        {
            return Result.Failure(Errors.Counterparty.FullNameRequired);
        }

        return Result.Success();
    }
}