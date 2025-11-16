namespace Onefocus.Common.Abstractions.ServiceBus.Membership;

public interface IBulkSearchIndexMessage
{
    IReadOnlyList<ISearchIndexMessage> Entities { get; }
}