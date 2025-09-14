using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Results;

namespace Onefocus.Identity.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public Guid MembershipUserId { get; private set; }

    private User(string email, Guid membershipUserId) : base(email)
    {
        Email = email;
        MembershipUserId = membershipUserId;
    }

    public static Result<User> Create(string email, Guid membershipUserId)
    {
        return new User(email, membershipUserId);
    }

    public void Update(string email, string? hashedPassword)
    {
        Email = email;

        if (!string.IsNullOrEmpty(hashedPassword))
        {
            PasswordHash = hashedPassword;
        }
    }
}

