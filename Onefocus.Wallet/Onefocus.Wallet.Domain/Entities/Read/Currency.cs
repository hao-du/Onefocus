using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Transactions;
using System.Transactions;
using static Onefocus.Wallet.Domain.Errors;

namespace Onefocus.Wallet.Domain.Entities.Read;

public class Currency : WriteEntityBase
{
    public string Name { get; init; } = default!;
    public string ShortName { get; init; } = default!;
    public bool DefaultFlag { get; init; }
}