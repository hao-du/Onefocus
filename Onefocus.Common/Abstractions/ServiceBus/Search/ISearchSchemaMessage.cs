using Onefocus.Common.Search.Schema;

namespace Onefocus.Common.Abstractions.ServiceBus.Search;

public interface ISearchSchemaMessage
{
    Guid EventId { get; }
    string SchemaName { get; }
    string IndexName { get; }
    MappingSchema Schema { get; }
    DateTime Timestamp { get; }
    Dictionary<string, string> Metadata { get; }
}
