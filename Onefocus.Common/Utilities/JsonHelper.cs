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

    public static string MinifyJson(object content)
    {
        if (content is null) return string.Empty;

        var json = content switch
        {
            JsonElement element => element.GetRawText(),
            string str => str,
            _ => string.Empty
        };

        if(string.IsNullOrWhiteSpace(json)) return string.Empty;

        using var doc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(doc.RootElement);
    }

    public static Dictionary<string, string> SplitJson(string originalJson, string[] keyPropertyNames)
    {
        var splitedJsonList = new List<(string property, string json)>();
        if (string.IsNullOrWhiteSpace(originalJson)) return splitedJsonList.ToDictionary();

        using var doc = JsonDocument.Parse(originalJson);
        var root = doc.RootElement;
        var opts = new JsonSerializerOptions { WriteIndented = true };

        foreach (var keyProperty in keyPropertyNames)
        {
            if (root.TryGetProperty(keyProperty, out JsonElement mappingsElement))
            {
                var serializedJson = JsonSerializer.Serialize(mappingsElement, opts);
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
