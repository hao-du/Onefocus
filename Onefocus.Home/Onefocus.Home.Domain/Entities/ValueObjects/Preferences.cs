using Onefocus.Common.Results;
using Onefocus.Common.Utilities;
using Onefocus.Home.Domain.Entities.Write.Params;

namespace Onefocus.Home.Domain.Entities.ValueObjects
{
    public class Preferences
    {
        public string Locale { get; private set; }
        public string Timezone { get; private set; }

        public Preferences(string locale, string timezone)
        {
            Locale = locale;
            Timezone = timezone;
        }

        public static Preferences Default() => new Preferences(CultureInfoHelper.DefaultLocale, CultureInfoHelper.DefaultTimeZoneId);

        public static Result<Preferences> Create(PreferenceParams preferenceParams)
        {
            var validationResult = Validate(preferenceParams);
            if(validationResult.IsFailure) return validationResult.Failure<Preferences>();

            return new Preferences(
                preferenceParams.Locale,
                preferenceParams.Timezone
            );
        }

        public Result Update(PreferenceParams preferenceParams)
        {
            var validationResult = Validate(preferenceParams);
            if (validationResult.IsFailure) return validationResult.Failure<Preferences>();

            Locale = preferenceParams.Locale;
            Timezone = preferenceParams.Timezone; 

            return Result.Success();
        }

        public static Result Validate(PreferenceParams preferenceParams)
        {
            if (string.IsNullOrEmpty(preferenceParams.Locale))
            {
                return Result.Failure(Errors.Preference.LocaleRequired);
            }
            if (string.IsNullOrEmpty(preferenceParams.Timezone))
            {
                return Result.Failure(Errors.Preference.TimezoneRequired);
            }

            return Result.Success();
        }
    }
}
