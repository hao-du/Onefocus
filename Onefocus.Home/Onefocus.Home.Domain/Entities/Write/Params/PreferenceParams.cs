namespace Onefocus.Home.Domain.Entities.Write.Params
{
    public class PreferenceParams(string locale, string timeZone)
    {
        public string Locale { get; private set; } = locale;
        public string Timezone { get; private set; } = timeZone;

        public static PreferenceParams Create(string locale, string timeZone)
        {
            return new PreferenceParams(locale, timeZone);
        }
    }
}
