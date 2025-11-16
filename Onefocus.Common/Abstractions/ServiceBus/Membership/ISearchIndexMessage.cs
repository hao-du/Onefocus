namespace Onefocus.Common.Abstractions.ServiceBus.Membership;

public interface ISearchIndexMessage
{
    string? EntityType { get; }
    string? EntityId { get; }
    string Payload { get; }
}