using Onefocus.Common.Results;

namespace Onefocus.Identity.Domain;

public static class Errors
{
    public static class User
    {
        public static readonly Error UserNotExist = new("UserNotExist", "User does not exist.");
        public static readonly Error EmailRequired = new ("EmailRequired", "Email is required.");
        public static readonly Error InvalidEmail = new("InvalidEmail", "Email is invalid.");
        public static readonly Error PasswordRequired = new("PasswordRequired", "Password is required.");
        public static readonly Error IncorrectUserNameOrPassword = new("IncorrectUserNameOrPassword", "Incorrect user name or password.");
    }

    public static class Token
    {
        public static readonly Error CannotCreateToken = new("CannotCreateToken", "Token is not able to be created.");
        public static readonly Error InvalidRefreshToken = new("InvalidRefreshToken", "Refresh token is invalid.");
    }
}