using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Home.Domain;
using Onefocus.Home.Domain.Entities.ValueObjects;
using Onefocus.Home.Domain.Entities.Write;

namespace Onefocus.Home.Domain.Entities.Read;

public sealed class Setting : ReadEntityBase
{
    public Guid UserId { get; private set; } = default!;
    public Preference Preference { get; private set; } = default!;

    public User User { get; private set; } = default!;
}