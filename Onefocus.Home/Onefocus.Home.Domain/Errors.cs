using Onefocus.Common.Results;

namespace Onefocus.Home.Domain;

public static class Errors
{
    public static class User
    {
        public static readonly Error FirstNameRequired = new("FirstNameRequired", "First name is required.");
        public static readonly Error LastNameRequired = new("LastNameRequired", "Last name is required.");
        public static readonly Error EmailRequired = new("EmailRequired", "Email is required.");
    }

    public static class Preference
    {
        public static readonly Error PreferenceRequired = new("PreferenceRequired", "Preference is required.");
        public static readonly Error LocaleRequired = new("LocaleRequired", "Locale is required.");
        public static readonly Error TimezoneRequired = new("TimezoneRequired", "Timezone is required.");
    }
}