using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Results;

namespace Onefocus.Identity.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public User(string userName) : base(userName)
    {
    }

    public static Result<User> Create(string email)
    {
        return new User(email);
    }

    public void Update(string email)
    {
        Email = email;
    }
}

