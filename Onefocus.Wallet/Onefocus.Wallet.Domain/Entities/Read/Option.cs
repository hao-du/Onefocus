using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Abstractions.Domain.Fields;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Interfaces;

namespace Onefocus.Wallet.Domain.Entities.Read;

public sealed class Option : ReadEntityBase, INameField, IOwnerUserField
{
    public string Name { get; init; } = default!;
    public Guid OwnerUserId { get; init; }
    public OptionType OptionType { get; init; }

    public User OwnerUser { get; init; } = default!;
}