using Microsoft.AspNetCore.Identity;

namespace Onefocus.Membership.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    private readonly List<UserRole> _userRoles = [];
    public virtual IReadOnlyList<UserRole> UserRoles => _userRoles;
}
