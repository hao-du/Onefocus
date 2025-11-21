namespace Onefocus.Common.Abstractions.ServiceBus.Search;

public interface ISearchIndexMessage
{
    IReadOnlyList<ISearchIndexDocument> Documents { get; }
}