namespace Onefocus.Common.Abstractions.ServiceBus.Search;

public interface ISearchIndexDocument
{
    string IndexName { get; }
    string DocumentId { get; }
    string Payload { get; }
    Dictionary<string, string> VectorSearchTerms { get; }
}