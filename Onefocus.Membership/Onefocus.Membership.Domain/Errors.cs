using Onefocus.Common.Results;

namespace Onefocus.Membership.Domain;

public static class Errors
{
    public static class User
    {
        public static readonly Error IdRequired = new("IdRequired", "User identity is required.");
        public static readonly Error FirstNameRequired = new("FirstNameRequired", "First name is required.");
        public static readonly Error LastNameRequired = new("LastNameRequired", "Last name is required.");
        public static readonly Error EmailRequired = new ("EmailRequired", "Email is required.");
        public static readonly Error InvalidEmail = new("InvalidEmail", "Email is invalid.");
        public static readonly Error UserNotExist = new("UserNotExist", "User does not exist.");
        public static readonly Error PasswordRequired = new("PasswordRequired", "Password is required.");
        public static readonly Error ConfirmPasswordRequired = new("ConfirmPasswordRequired", "Confirm password is required.");
        public static readonly Error PasswordNotMatchConfirmPassword = new("PasswordNotMatchConfirmPassword", "Confirm password does not match password.");
    }

    public static class UserRole
    {
        public static readonly Error UserRequired = new("UserRequired", "User is required.");
        public static readonly Error RoleRequired = new("RoleRequired", "Role is required.");
        public static readonly Error UserIdRequired = new("UserIdRequired", "User identity is required.");
        public static readonly Error RoleIdRequired = new("RoleIdRequired", "Role identity is required.");
    }
}