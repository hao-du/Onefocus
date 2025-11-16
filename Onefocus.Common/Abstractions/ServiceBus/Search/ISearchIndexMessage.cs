namespace Onefocus.Common.Abstractions.ServiceBus.Search;

public interface ISearchIndexMessage
{
    IReadOnlyList<ISearchIndexEntity> Entities { get; }
}