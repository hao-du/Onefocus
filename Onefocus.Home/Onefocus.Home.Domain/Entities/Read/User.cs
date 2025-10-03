using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Home.Domain.Entities.Read;

public sealed class User : ReadEntityBase
{
    public Guid? SettingId { get; init; } = default!;

    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;

    public Setting Setting { get; init; } = default!;

    public string GetFullName() => $"{FirstName} {LastName}".Trim();
}