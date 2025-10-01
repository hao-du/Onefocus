using System.Globalization;

namespace Onefocus.Common.Utilities
{
    public static class CultureInfoHelper
    {
        public const string DefaultLocale = "en-US";
        public const string DefaultTimeZoneId = "UTC";

        public static readonly CultureInfo DefaultCulture = CultureInfo.GetCultureInfo(DefaultLocale);
        public static readonly TimeZoneInfo DefaultTimeZone = TimeZoneInfo.Utc;

        public static IEnumerable<CultureInfo> GetAllLocales()
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        }

        public static IEnumerable<TimeZoneInfo> GetAllTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }

        public static CultureInfo GetCultureOrDefault(string localeCode)
        {
            try
            {
                return CultureInfo.GetCultureInfo(localeCode);
            }
            catch (CultureNotFoundException)
            {
                return DefaultCulture;
            }
        }

        public static TimeZoneInfo GetTimeZoneOrDefault(string timeZoneId)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                return DefaultTimeZone;
            }
            catch (InvalidTimeZoneException)
            {
                return DefaultTimeZone;
            }
        }
    }
}
