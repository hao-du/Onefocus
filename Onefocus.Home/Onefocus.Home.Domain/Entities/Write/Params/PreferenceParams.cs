namespace Onefocus.Home.Domain.Entities.Write.Params
{
    public class PreferenceParams(string locale, string timezone)
    {
        public string Locale { get; private set; } = locale;
        public string Timezone { get; private set; } = timezone;

        public static PreferenceParams Create(string locale, string timezone)
        {
            return new PreferenceParams(locale, timezone);
        }
    }
}
