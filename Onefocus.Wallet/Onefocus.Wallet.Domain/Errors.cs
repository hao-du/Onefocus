using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain;

public static class Errors
{
    public static class User
    {
        public static readonly Error FirstNameRequired = new("FirstNameRequired", "First name is required.");
        public static readonly Error LastNameRequired = new("LastNameRequired", "Last name is required.");
        public static readonly Error EmailRequired = new("EmailRequired", "Email is required.");
        public static readonly Error UserNotExist = new("UserNotExist", "User does not exist.");
    }
}