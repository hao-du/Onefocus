using Onefocus.Common.Results;

namespace Onefocus.Identity.Infrastructure.Security;

public static class Errors
{
    public static class TokenService
    {
        public static readonly Error RolesRequired = new("RolesRequired", "Roles are required.");
        public static readonly Error EmailRequired = new("EmailRequired", "Email is required.");
    }
}