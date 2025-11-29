using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Onefocus.Common.Utilities;

public static class JsonHelper
{
    public static JsonSerializerOptions GetOptions()
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        return options;
    }

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

    public static string SerializeJson(object content)
    {
        if (content is null) return string.Empty;

        var json = content switch
        {
            JsonElement element => element.GetRawText(),
            string str => str,
            _ => string.Empty
        };

        using var doc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(doc.RootElement, GetOptions());
    }

    public static Dictionary<string, string> GetSections(string originalJson, string[] keyPropertyNames)
    {
        var splitedJsonList = new List<(string property, string json)>();
        if (string.IsNullOrWhiteSpace(originalJson)) return splitedJsonList.ToDictionary();

        using var doc = JsonDocument.Parse(originalJson);
        var root = doc.RootElement;

        foreach (var keyProperty in keyPropertyNames)
        {
            if (root.TryGetProperty(keyProperty, out JsonElement mappingsElement))
            {
                var serializedJson = SerializeJson(mappingsElement);
                splitedJsonList.Add(new()
                {
                    property = keyProperty,
                    json = serializedJson
                });
            }
        }

        return splitedJsonList.ToDictionary();
    }
}
