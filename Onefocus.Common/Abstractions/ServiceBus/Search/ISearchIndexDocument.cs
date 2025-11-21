namespace Onefocus.Common.Abstractions.ServiceBus.Search;

public interface ISearchIndexDocument
{
    string? IndexName { get; }
    string? DocumentId { get; }
    object Payload { get; }
}