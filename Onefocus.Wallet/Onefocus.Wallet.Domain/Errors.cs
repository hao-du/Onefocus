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
        public static readonly Error UserRequired = new("UserRequired", "User is required.");
        public static readonly Error AuthenticationIdRequired = new("AuthenticationIdRequired", "AuthenticationId is required.");
    }

    public static class Currency
    {
        public static readonly Error NameRequired = new("CurrencyNameRequired", "Name is required.");
        public static readonly Error ShortNameRequired = new("CurrencyShortNameRequired", "Short name is required.");
        public static readonly Error CurrencyRequired = new("CurrencyRequired", "Currency is required.");
        public static readonly Error ShortNameLengthMustBeThreeOrFour = new("ShortNameLengthMustBeThreeOrFour", "Short name must have 3 to 4 characters.");
        public static readonly Error NameOrShortNameIsExisted = new("NameOrShortNameIsExisted", "Either name or short name is existed.");
    }

    public static class Bank
    {
        public static readonly Error NameRequired = new("BankNameRequired", "Bank name is required.");
        public static readonly Error BankRequired = new("BankRequired", "Bank is required.");
    }

    public static class BankAccount
    {
        public static readonly Error AccountNumberRequired = new("AccountNumberRequired", "Account number is required.");
        public static readonly Error ClosedOnRequired = new("ClosedOnRequired", "Close account date is required.");
        public static readonly Error IssuedOnRequired = new("IssuedOnRequired", "Issued date is required.");
    }

    public static class Transaction
    {
        public static readonly Error AmountMustEqualOrGreaterThanZero = new("AmountMustEqualOrGreaterThanZero", "Amount must be equal or greater than 0.");
        public static readonly Error BankAccountExists = new("BankAccountExists", "Bank account exists.");
    }

    public static class CurrencyExchange
    {
        public static readonly Error BaseCurrencyRequired = new("BaseCurrencyRequired", "Base currency is required.");
        public static readonly Error TargetCurrencyRequired = new("TargetCurrencyRequired", "Target currency is required.");
        public static readonly Error ExchangeRateMustGreaterThanZero = new("ExchangeRateMustGreaterThanZero", "Exchange rate must be greater than 0.");
    }

    public static class TransactionItem
    {
        public static readonly Error ItemNameRequired = new("ItemNameRequired", "Item name is required.");
    }
}