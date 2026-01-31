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

    public static T DeserializeJson<T>(string? json) where T : new()
    {
        if (string.IsNullOrEmpty(json)) return new();

        var value = JsonSerializer.Deserialize<T>(json, GetOptions());
        return value ?? new T();
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

    public static string AppendEmbeddings(string originalJson, Dictionary<string, string> vectorTerms, Dictionary<string, List<float>> embeddings)
    {
        var json = originalJson;
        if (embeddings.Count > 0)
        {
            foreach (var term in vectorTerms)
            {
                var vectors = embeddings[term.Value];
                if (vectors != null && vectors.Count > 0)
                {
                    string floatJson = JsonSerializer.Serialize(vectors);
                    json = originalJson.Replace($"\"{term.Key}\"", floatJson, StringComparison.InvariantCultureIgnoreCase);
                }
            }
        }

        return json;
    }
}
