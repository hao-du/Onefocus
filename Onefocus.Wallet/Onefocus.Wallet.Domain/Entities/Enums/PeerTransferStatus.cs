using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Enums
{
    public enum PeerTransferStatus
    {
        [Description("Completed")]
        Completed = 100,
        [Description("Failed")]
        Failed = 200,
        [Description("Processing")]
        Processing = 300,
        [Description("Pending")]
        Pending = 400
    }
}
