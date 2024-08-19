using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Abstractions.Domain
{
    public abstract class EntityBase
    {
        public Guid Id { get; private set; }
        public bool ActiveFlag { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public Guid CreatedBy { get; private set; }
        public Guid UpdatedBy { get; private set; }

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
