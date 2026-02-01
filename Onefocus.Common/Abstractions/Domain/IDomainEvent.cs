namespace Onefocus.Common.Abstractions.Domain;

public interface IDomainEvent
{
    string EventType { get; }
    string ObjectName { get; }
    string ObjectId { get; }
    string Payload { get; }
    Dictionary<string, string> KeyValuePairs { get; }
}