using Onefocus.Common.Utilities.Attributes;
using Onefocus.Wallet.Domain.Entities.Read.Transactions;
using System.ComponentModel;

namespace Onefocus.Wallet.Domain.Entities.Enums
{
    public enum Action
    {
        [Description("Get")]
        [Group(typeof(IncomeTransaction))]
        Get = 0,

        [Description("Pay")]
        [Group(typeof(OutcomeTransaction))]
        Pay = 1,

        [Description("Lend")]
        [Group(typeof(TransferTransaction))]
        Lend = 2,

        [Description("Get back")]
        [Group(typeof(TransferTransaction))]
        GetBack = 3,

        [Description("Borrow")]
        [Group(typeof(TransferTransaction))]
        Borrow = 4,

        [Description("Return")]
        [Group(typeof(TransferTransaction))]
        Return = 5,

        [Description("Give")]
        [Group(typeof(TransferTransaction))]
        Give = 6,

        [Description("Receive")]
        [Group(typeof(TransferTransaction))]
        Receive = 7,

        [Description("Withdraw")]
        [Group(typeof(BankingTransaction))]
        Withdraw = 8,

        [Description("Deposit")]
        [Group(typeof(BankingTransaction))]
        Deposit = 9,

        [Description("Get interest")]
        [Group(typeof(BankingTransaction))]
        GetInterest = 10,

        [Description("Exchange from")]
        [Group(typeof(ExchangeTransaction))]
        ExchangeFrom = 11,

        [Description("Exchange to")]
        [Group(typeof(ExchangeTransaction))]
        ExchangeTo = 12,
    }
}
