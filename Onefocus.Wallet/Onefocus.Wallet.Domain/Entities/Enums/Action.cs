using Onefocus.Common.Utilities.Attributes;
using System.ComponentModel;

namespace Onefocus.Wallet.Domain.Entities.Enums
{
    public enum Action
    {
        [Description("Get")]
        [Group<TransactionType>(TransactionType.Income)]
        Get = 0,

        [Description("Pay")]
        [Group<TransactionType>(TransactionType.Outcome)]
        Pay = 1,

        [Description("Lend")]
        [Group<TransactionType>(TransactionType.Transfer)]
        Lend = 2,

        [Description("Get back")]
        [Group<TransactionType>(TransactionType.Transfer)]
        GetBack = 3,

        [Description("Borrow")]
        [Group<TransactionType>(TransactionType.Transfer)]
        Borrow = 4,

        [Description("Return")]
        [Group<TransactionType>(TransactionType.Transfer)]
        Return = 5,

        [Description("Withdraw")]
        [Group<TransactionType>(TransactionType.Banking)]
        Withdraw = 6,

        [Description("Deposit")]
        [Group<TransactionType>(TransactionType.Banking)]
        Deposit = 7,

        [Description("Get interest")]
        [Group<TransactionType>(TransactionType.Banking)]
        GetInterest = 8,

        [Description("Exchange from")]
        [Group<TransactionType>(TransactionType.Exchange)]
        ExchangeFrom = 9,

        [Description("Exchange to")]
        [Group<TransactionType>(TransactionType.Exchange)]
        ExchangeTo = 10,
    }
}
