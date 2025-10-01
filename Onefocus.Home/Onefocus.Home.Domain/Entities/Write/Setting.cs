using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Home.Domain;
using Onefocus.Home.Domain.Entities.Read;
using Onefocus.Home.Domain.Entities.ValueObjects;
using Onefocus.Home.Domain.Entities.Write;

namespace Onefocus.Home.Domain.Entities.Write;

public sealed class Setting : WriteEntityBase, IAggregateRoot
{
    public Guid UserId { get; private set; } = default!;
    public Preference Preference { get; private set; } = default!;

    public User User { get; private set; } = default!;

    private Setting()
    {
        // Required for EF Core
    }

    private Setting(Guid? id, Preference preference, string? description, Guid actionedBy)
    {
        Init(id ?? Guid.NewGuid(), description, actionedBy);

        Preference = preference;
    }

    public static Result<Setting> Create(Guid? id, Preference preference, string? description, Guid actionedBy)
    {
        var validationResult = Validate(preference);
        if (validationResult.IsFailure) return (Result<Setting>)validationResult;

        return new Setting(id, preference, description, actionedBy);
    }

    public Result Update(Preference preference, string? description, bool isActive, Guid actionedBy)
    {
        var validationResult = Validate(preference);
        if (validationResult.IsFailure)
        {
            return Result.Failure<User>(validationResult.Errors);
        }

        Preference = preference;
        Description = description;

        SetActiveFlag(isActive, actionedBy);

        return Result.Success();
    }

    private static Result Validate(Preference preference)
    {
        if(preference is null)
        {
            return Result.Failure(Errors.Preference.PreferenceRequired);
        }

        return Result.Success();
    }
}