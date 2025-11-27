namespace Onefocus.Common.Abstractions.Domain;

public interface IDomainEvent
{
    string IndexName { get; }
    string EntityId { get; }
    string EventType { get; }
    object Payload { get; }
}

public interface IDomainEvent<T> : IDomainEvent
{
    T Entity { get; }
}