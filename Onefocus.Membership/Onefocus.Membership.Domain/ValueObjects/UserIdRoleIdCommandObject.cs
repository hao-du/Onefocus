using Onefocus.Membership.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Membership.Domain.ValueObjects
{
    public sealed record UserIdRoleIdCommandObject(Guid UserId, Guid RoleId);
}
