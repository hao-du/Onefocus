namespace Onefocus.Common.Abstractions.Domain;

public interface IDomainEvent
{
    string EntityName { get; }
    string EntityId { get; }
    string EventType { get; }
    string Payload { get; }
}

public interface IDomainEvent<T> : IDomainEvent
{
    T Entity { get; }
}