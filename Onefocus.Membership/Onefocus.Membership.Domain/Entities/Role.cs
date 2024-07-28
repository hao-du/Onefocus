using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain.ValueObjects;

namespace Onefocus.Membership.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    private readonly List<UserRole> _userRoles = new();
    public virtual IReadOnlyList<UserRole> UserRoles => _userRoles;
}
