using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using System.Security.Cryptography.X509Certificates;

namespace Onefocus.Wallet.Domain.Entities.Read;

public class Bank : ReadEntityBase
{
    public string Name { get; init; } = default!;
}