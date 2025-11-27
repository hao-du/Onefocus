namespace Onefocus.Common.Abstractions.ServiceBus.Search;

public interface ISearchSchemaMessage
{
    string IndexName { get; }
    string Mappings { get; }
}
