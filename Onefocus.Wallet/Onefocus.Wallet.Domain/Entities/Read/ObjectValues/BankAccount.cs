﻿namespace Onefocus.Wallet.Domain.Entities.Read.ObjectValues
{
    public sealed class BankAccount
    {
        public string AccountNumber { get; init; } = default!;
        public DateTimeOffset ClosedOn { get; init; }
        public bool CloseFlag { get; init; }
    }
}
