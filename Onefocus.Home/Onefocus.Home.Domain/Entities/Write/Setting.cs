using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Home.Domain;
using Onefocus.Home.Domain.Entities.Read;
using Onefocus.Home.Domain.Entities.ValueObjects;
using Onefocus.Home.Domain.Entities.Write.Params;

namespace Onefocus.Home.Domain.Entities.Write;

public sealed class Setting : WriteEntityBase, IAggregateRoot
{
    public Guid UserId { get; private set; } = default!;
    public Preferences Preferences { get; private set; } = default!;

    public User User { get; private set; } = default!;

    private Setting()
    {
        // Required for EF Core
    }

    private Setting(Guid actionedBy)
    {
        Init(null, null, actionedBy);
    }

    public static Result<Setting> Create(PreferenceParams preferenceParams, Guid actionedBy)
    {
        var validationResult = Validate(preferenceParams);
        if (validationResult.IsFailure) return (Result<Setting>)validationResult;

        var preferencesResult = Preferences.Create(preferenceParams);
        if (preferencesResult.IsFailure) return preferencesResult.Failure<Setting>();

        var setting = new Setting(actionedBy);
        setting.UserId = actionedBy;
        setting.Preferences = preferencesResult.Value;
        return setting;
    }

    public Result Update(PreferenceParams preferenceParams, Guid actionedBy)
    {
        var validationResult = Validate(preferenceParams);
        if (validationResult.IsFailure) return validationResult;

        if (Preferences == null)
        {
            var preferencesResult = Preferences.Create(preferenceParams);
            if (preferencesResult.IsFailure) return preferencesResult.Failure<Setting>();

            UserId = actionedBy;
            Preferences = preferencesResult.Value;
        }
        else
        {
            Preferences.Update(preferenceParams);
        }

        return Result.Success();
    }

    private static Result Validate(PreferenceParams preferenceParams)
    {
        if(preferenceParams is null)
        {
            return Result.Failure(Errors.Preference.PreferencesRequired);
        }

        return Result.Success();
    }
}