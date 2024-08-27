using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write.ObjectValues
{
    public sealed class BankAccount
    {
        public string AccountNumber { get; private set; }
        public DateTimeOffset ClosedOn { get; private set; }
        public bool CloseFlag { get; private set; } = false;

        private BankAccount(string accountNumber, DateTimeOffset closedOn, bool closeFlag)
        {
            AccountNumber = accountNumber;
            ClosedOn = closedOn;
            CloseFlag = closeFlag;
        }

        public static Result<BankAccount> Create(string accountNumber, DateTimeOffset closedOn, bool closeFlag)
        {
            var validationResult = Validate(accountNumber);
            if (validationResult.IsFailure)
            {
                return Result.Failure<BankAccount>(validationResult.Error);
            }

            return new BankAccount(accountNumber, closedOn, closeFlag);
        }

        public Result Update(string accountNumber, DateTimeOffset closedOn, bool closeFlag)
        {
            var validationResult = Validate(accountNumber);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            AccountNumber = accountNumber;
            ClosedOn = closedOn;
            CloseFlag = closeFlag;

            return Result.Success();
        }

        public void SetCloseFlag(bool closeFlag)
        {
            CloseFlag = closeFlag;
        }

        private static Result Validate(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                return Result.Failure(Errors.BankAccount.AccountNumberRequired);
            }

            return Result.Success();
        }
    }
}
