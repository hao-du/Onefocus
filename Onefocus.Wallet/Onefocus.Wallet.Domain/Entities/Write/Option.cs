using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Abstractions.Domain.Fields;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Interfaces;

namespace Onefocus.Wallet.Domain.Entities.Write;

public sealed class Option : WriteEntityBase, INameField, IOwnerUserField, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public Guid OwnerUserId { get; private set; }
    public OptionType OptionType { get; private set; }

    public User OwnerUser { get; private set; } = default!;


    private Option()
    {
    }

    private Option(string name, string? description, Guid ownerId, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Name = name;
        OwnerUserId = ownerId;
    }

    public static Result<Option> Create(string name, string? description, Guid ownerId, Guid actionedBy)
    {
        var validationResult = Validate(name);
        if (validationResult.IsFailure)
        {
            return Result.Failure<Option>(validationResult.Errors);
        }

        return new Option(name, description, ownerId, actionedBy);
    }

    public Result Update(string name, string? description, bool isActive, Guid actionedBy)
    {
        var validationResult = Validate(name);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Name = name;
        Description = description;

        if (isActive) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return Result.Success();
    }

    private static Result Validate(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<Bank>(Errors.Option.NameRequired);
        }

        return Result.Success();
    }
}