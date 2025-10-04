using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Home.Domain;
using Onefocus.Home.Domain.Entities.Read;
using Onefocus.Home.Domain.Entities.ValueObjects;
using Onefocus.Home.Domain.Entities.Write.Params;

namespace Onefocus.Home.Domain.Entities.Write;

public sealed class Settings : WriteEntityBase, IAggregateRoot
{
    public Guid UserId { get; private set; } = default!;
    public Preferences Preferences { get; private set; } = default!;

    public User User { get; private set; } = default!;

    private Settings()
    {
        // Required for EF Core
    }

    private Settings(Guid actionedBy)
    {
        Init(null, null, actionedBy);
    }

    public static Result<Settings> Create(PreferenceParams preferenceParams, Guid actionedBy)
    {
        var validationResult = Validate(preferenceParams);
        if (validationResult.IsFailure) return (Result<Settings>)validationResult;

        var preferencesResult = Preferences.Create(preferenceParams);
        if (preferencesResult.IsFailure) return preferencesResult.Failure<Settings>();

        var settings = new Settings(actionedBy);
        settings.UserId = actionedBy;
        settings.Preferences = preferencesResult.Value;
        return settings;
    }

    public Result Update(PreferenceParams preferenceParams, Guid actionedBy)
    {
        var validationResult = Validate(preferenceParams);
        if (validationResult.IsFailure) return validationResult;

        if (Preferences == null)
        {
            var preferencesResult = Preferences.Create(preferenceParams);
            if (preferencesResult.IsFailure) return preferencesResult.Failure<Settings>();

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