using Onefocus.Home.Domain.Entities.ValueObjects;

namespace Onefocus.Home.Application.Contracts.Write.Settings;

public sealed record UpdateSettingsRequestDto(Guid Id, Guid UserId, Preferences preferences, bool IsActive, Guid ActionedBy);