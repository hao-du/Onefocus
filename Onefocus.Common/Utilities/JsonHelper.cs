using System.Text.Json;

namespace Onefocus.Common.Utilities;

public static class JsonHelper
{
    public static bool IsValidJson(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return false;

        try
        {
            using (JsonDocument.Parse(content))
            {
                return true;
            }
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
