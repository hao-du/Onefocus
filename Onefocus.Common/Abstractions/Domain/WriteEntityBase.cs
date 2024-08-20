﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Abstractions.Domain
{
    public abstract class WriteEntityBase: EntityBase
    {
        protected void Init(Guid? id, Guid actionedBy)
        {
            Id = id.HasValue ? id.Value : Guid.NewGuid();
            ActiveFlag = true;
            CreatedBy = actionedBy;
            CreatedOn = DateTimeOffset.Now;
            Update(actionedBy);
        }

        protected void Update(Guid actionedBy)
        {
            UpdatedOn = DateTimeOffset.Now;
            UpdatedBy = actionedBy;
        }

        protected void MarkActive(Guid actionedBy)
        {
            ActiveFlag = true;
            Update(actionedBy);
        }

        protected void MarkInactive(Guid actionedBy)
        {
            ActiveFlag = false;
            Update(actionedBy);
        }
    }
}