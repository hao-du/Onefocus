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
    }

    public static class Currency
    {
        public static readonly Error NameRequired = new("CurrencyNameRequired", "Name is required.");
        public static readonly Error ShortNameRequired = new("CurrencyShortNameRequired", "Short name is required.");
        public static readonly Error CurrencyRequired = new("CurrencyRequired", "Currency is required.");
    }

    public static class Bank
    {
        public static readonly Error NameRequired = new("BankNameRequired", "Bank name is required.");
        public static readonly Error BankRequired = new("BankRequired", "Bank is required.");
    }

    public static class BankAccount
    {
        public static readonly Error AccountNumberRequired = new("AccountNumberRequired", "Account number is required.");
    }

    public static class Transaction
    {
        public static readonly Error AmountMustGreaterThanZero = new("AmountMustGreaterThanZero", "Amount must be greater than 0.");

        public static class Transfer 
        {
            public static readonly Error TransferredUserRequired = new("TransferredUserRequired", "Transferred user is required.");
            public static readonly Error RequireDefaultActionInDetailList = new("RequireDefaultActionInDetailList", "Default action must be defined in transaction action list.");
        }

        public static class Exchange
        {
            public static readonly Error ExchangedCurrencyRequired = new("ExchangedCurrencyRequired", "Exchanged currency is required.");
            public static readonly Error ExchangeRateMustGreaterThanZero = new("ExchangeRateMustGreaterThanZero", "Exchange rate must be greater than 0.");
        }

        public static class Detail
        {
            public static readonly Error DetailRequired = new("DetailRequired", "Cannot insert empty transaction action.");
            public static readonly Error DetailMustBeNew = new("DetailMustBeNew", "Cannot insert an existing transaction action.");
        }
    }
}