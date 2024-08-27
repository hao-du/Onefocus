using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Abstractions.Domain
{
    public abstract class EntityBase
    {
        public Guid Id { get; protected set; }
        public string? Description { get; protected set; }
        public bool ActiveFlag { get; protected set; }
        public DateTimeOffset? CreatedOn { get; protected set; }
        public DateTimeOffset? UpdatedOn { get; protected set; }
        public Guid? CreatedBy { get; protected set; }
        public Guid? UpdatedBy { get; protected set; }
    }
}
