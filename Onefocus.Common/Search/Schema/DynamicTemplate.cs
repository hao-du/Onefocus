namespace Onefocus.Common.Search.Schema;

public class DynamicTemplate
{
    public required string Name { get; set; }
    public required string Match { get; set; } // Pattern to match field names
    public string? UnMatch { get; set; } // Pattern to exclude
    public required string MatchMappingType { get; set; } // "string", "long", etc.
    public string? PathMatch { get; set; }
    public required FieldMapping Mapping { get; set; }
}
