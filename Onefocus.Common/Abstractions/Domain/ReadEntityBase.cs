using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Abstractions.Domain
{
    public abstract class ReadEntityBase
    {
        public Guid Id { get; init; }
        public bool ActiveFlag { get; init; }
        public DateTimeOffset CreatedOn { get; init; }
        public DateTimeOffset UpdatedOn { get; init; }
        public Guid CreatedBy { get; init; }
        public Guid UpdatedBy { get; init; }
    }
}
