namespace Onefocus.Common.Search.Schema;

public class FieldMapping
{
    public required string Type { get; set; } // "text", "keyword", "date", "boolean", "long", "nested"
    public bool? Index { get; set; } // Whether field is indexed
    public string? Analyzer { get; set; } // For text fields
    public Dictionary<string, FieldMapping>? Fields { get; set; } // Multi-fields (e.g., text + keyword)
    public Dictionary<string, FieldMapping>? NestedProperties { get; set; } // For nested type
    public string? Format { get; set; } // For date fields
    public Dictionary<string, object>? AdditionalProperties { get; set; } // Any extra config
}
