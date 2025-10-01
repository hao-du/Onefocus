using Onefocus.Common.Results;
using Onefocus.Common.Utilities;

namespace Onefocus.Home.Domain.Entities.ValueObjects
{
    public class Preference
    {
        public string Locale { get; private set; }
        public string Timezone { get; private set; }

        public Preference(string locale, string timezone)
        {
            Locale = locale;
            Timezone = timezone;
        }

        public static Result<Preference> Default() => new Preference(CultureInfoHelper.DefaultLocale, CultureInfoHelper.DefaultTimeZoneId);

        public static Result<Preference> Create(string locale, string timezone)
        {
            var validationResult = Validate(locale, timezone);
            if(validationResult.IsFailure) return validationResult.Failure<Preference>();

            return new Preference(locale, timezone);
        }

        public Result Update(string locale, string timezone)
        {
            var validationResult = Validate(locale, timezone);
            if (validationResult.IsFailure) return validationResult.Failure<Preference>();

            Locale = locale;
            Timezone = timezone; 

            return Result.Success();
        }

        private static Result Validate(string locale, string timezone)
        {
            if (string.IsNullOrEmpty(locale))
            {
                return Result.Failure(Errors.Preference.LocaleRequired);
            }
            if (string.IsNullOrEmpty(timezone))
            {
                return Result.Failure(Errors.Preference.TimezoneRequired);
            }

            return Result.Success();
        }
    }
}
