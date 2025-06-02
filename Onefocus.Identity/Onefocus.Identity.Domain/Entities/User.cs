using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Results;

namespace Onefocus.Identity.Domain.Entities;

public class User(string userName, Guid membershipUserId) : IdentityUser<Guid>(userName)
{
    public Guid MembershipUserId { get; private set; } = membershipUserId;

    public static Result<User> Create(string email, Guid membershipUserId)
    {
        return new User(email, membershipUserId);
    }

    public void Update(string email)
    {
        Email = email;
    }
}

