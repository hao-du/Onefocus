using Onefocus.Common.Results;
using Onefocus.Common.Utilities;

namespace Onefocus.Common.Abstractions.Domain;

public abstract class WriteEntityBase : EntityBase
{
    private List<IDomainEvent> _domainEvents => [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void Init(Guid? id, string? description, Guid actionedBy)
    {
        Id = id ?? Guid.Empty;
        Description = description;
        IsActive = true;
        CreatedBy = actionedBy;
        CreatedOn = DateTimeExtensions.Now();
        Update(actionedBy);
    }

    protected void Update(Guid actionedBy)
    {
        UpdatedOn = DateTimeExtensions.Now();
        UpdatedBy = actionedBy;
    }

    public void SetActiveFlag(bool isActive, Guid actionedBy)
    {
        IsActive = isActive;
        Update(actionedBy);
    }

    public void AddDomainEvent(IDomainEvent @event) {
        _domainEvents.Add(@event);
    }
}
