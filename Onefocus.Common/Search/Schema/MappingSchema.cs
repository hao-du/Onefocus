namespace Onefocus.Common.Search.Schema;

public class MappingSchema
{
    public required string SchemaName { get; set; }
    public  required string IndexName { get; set; }
    public required Dictionary<string, FieldMapping> Fields { get; set; }
    public required List<DynamicTemplate> DynamicTemplates { get; set; }
    public required SchemaSettings Settings { get; set; }
}
