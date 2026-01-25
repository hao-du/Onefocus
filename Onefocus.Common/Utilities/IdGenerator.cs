using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Onefocus.Common.Utilities;

public class IdGenerator : ValueGenerator<Guid>
{
    public override Guid Next(EntityEntry entry)
    {
        return Guid.CreateVersion7();
    }

    public override bool GeneratesTemporaryValues => false;
}
