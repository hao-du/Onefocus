namespace Onefocus.Common.Abstractions.ServiceBus.Search;

public interface ISearchIndexEntity
{
    string? IndexName { get; }
    string? EntityId { get; }
    string Payload { get; }
}