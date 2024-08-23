using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Onefocus.Wallet.Domain.Errors;
using Onefocus.Common.Abstractions.Domain;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Entities.ObjectValues
{
    public class BankAccount
    {
        public string AccountNumber { get; private set; }
        public DateTimeOffset? ClosedOn { get; set; }
        public bool CloseFlag { get; set; } = false;
        public Guid BankId { get; private set; }

        private BankAccount(string accountNumber, Guid bankId, DateTimeOffset? closedOn, bool closeFlag)
        {
            AccountNumber = accountNumber;
            ClosedOn = closedOn;
            BankId = bankId;
            CloseFlag = closeFlag;
        }

        public static Result<BankAccount> Create(string accountNumber, Guid bankId, DateTimeOffset? closedOn, bool closeFlag)
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

        public Result<BankAccount> Update(string accountNumber, Guid bankId, DateTimeOffset? closedOn, bool closeFlag)
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
