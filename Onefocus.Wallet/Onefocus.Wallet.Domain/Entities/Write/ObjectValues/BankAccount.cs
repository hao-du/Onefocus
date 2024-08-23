using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write.ObjectValues
{
    public sealed class BankAccount
    {
        public string AccountNumber { get; private set; }
        public DateTimeOffset ClosedOn { get; private set; }
        public bool CloseFlag { get; private set; } = false;
        public Guid BankId { get; private set; }

        public Bank Bank { get; private set; } = default!;

        private BankAccount(string accountNumber, Guid bankId, DateTimeOffset closedOn, bool closeFlag)
        {
            AccountNumber = accountNumber;
            ClosedOn = closedOn;
            BankId = bankId;
            CloseFlag = closeFlag;
        }

        public static Result<BankAccount> Create(string accountNumber, Guid bankId, DateTimeOffset closedOn, bool closeFlag)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                return Result.Failure<BankAccount>(Errors.BankAccount.AccountNumberRequired);
            }
            if (bankId == Guid.Empty)
            {
                return Result.Failure<BankAccount>(Errors.BankAccount.BankRequired);
            }

            return new BankAccount(accountNumber, bankId, closedOn, closeFlag);
        }

        public Result<BankAccount> Update(string accountNumber, Guid bankId, DateTimeOffset closedOn, bool closeFlag)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                return Result.Failure<BankAccount>(Errors.BankAccount.AccountNumberRequired);
            }
            if (bankId == Guid.Empty)
            {
                return Result.Failure<BankAccount>(Errors.BankAccount.BankRequired);
            }

            AccountNumber = accountNumber;
            BankId = bankId;
            ClosedOn = closedOn;
            CloseFlag = closeFlag;

            return this;
        }

        public void SetCloseFlag(bool closeFlag)
        {
            CloseFlag = closeFlag;
        }
    }
}
