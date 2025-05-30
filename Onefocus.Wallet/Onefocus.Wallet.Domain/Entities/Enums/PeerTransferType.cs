using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Enums
{
    public enum PeerTransferType
    {
        [Description("Lend")]
        Lend = 100,
        [Description("Borrow")]
        Borrow = 200,
        [Description("Give")]
        Give = 300,
        [Description("Receive")]
        Receive = 400,
    }
}
