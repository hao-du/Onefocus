namespace Onefocus.Common.Search.Schema;

public class SchemaSettings
{
    public int NumberOfShards { get; set; } = 1;
    public int NumberOfReplicas { get; set; } = 1;
    public string RefreshInterval { get; set; } = "1s";
}
