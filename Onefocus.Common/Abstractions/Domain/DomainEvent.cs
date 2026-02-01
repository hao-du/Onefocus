namespace Onefocus.Common.Abstractions.Domain;

public class DomainEvent : IDomainEvent
{
    public string EventType { get; private set; } = default!;
    public string ObjectName { get; private set; } = default!;
    public string ObjectId { get; private set; } = default!;
    public string Payload { get; private set; } = default!;
    public Dictionary<string, string> KeyValuePairs { get; private set; } = [];

    public static DomainEvent Create(
        string eventType,
        string objectName,
        string objectId,
        string payload,
        Dictionary<string, string> keyValuePairs
    )
    {
        var domainEvent = new DomainEvent();
        domainEvent.EventType = eventType;
        domainEvent.ObjectName = objectName;
        domainEvent.ObjectId = objectId;
        domainEvent.Payload = payload;
        domainEvent.KeyValuePairs = keyValuePairs;

        return domainEvent;
    }
}