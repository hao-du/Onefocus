using System.ComponentModel;

namespace Onefocus.Wallet.Domain.Entities.Enums
{
    public enum PeerTransferType
    {
        [Description("Lent")]
        Lend = 100,
        [Description("Borrowed")]
        Borrow = 200,
        [Description("Gave")]
        Give = 300,
        [Description("Received")]
        Receive = 400,
    }
}
