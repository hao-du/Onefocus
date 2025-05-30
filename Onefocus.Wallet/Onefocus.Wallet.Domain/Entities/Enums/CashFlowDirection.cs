using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Enums
{
    public enum CashFlowDirection
    {
        [Description("Income")]
        Income = 100,
        [Description("Expense")]
        Expense = 200
    }
}
