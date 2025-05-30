using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Abstractions.Domain;

public abstract class WriteEntityBase : EntityBase
{
    protected void Init(Guid? id, string? description, Guid actionedBy)
    {
        Id = id.HasValue ? id.Value : Guid.NewGuid();
        Description = description;
        IsActive = true;
        CreatedBy = actionedBy;
        CreatedOn = DateTimeOffset.Now;
        Update(actionedBy);
    }

    protected void Update(Guid actionedBy)
    {
        UpdatedOn = DateTimeOffset.Now;
        UpdatedBy = actionedBy;
    }

    public void MarkActive(Guid actionedBy)
    {
        IsActive = true;
        Update(actionedBy);
    }

    public void MarkInactive(Guid actionedBy)
    {
        IsActive = false;
        Update(actionedBy);
    }
}
