using Onefocus.Home.Domain.Entities.ValueObjects;

namespace Onefocus.Home.Application.Contracts.Write.Setting;

public sealed record UpdateSettingRequestDto(Guid Id, Guid UserId, Preferences preferences, bool IsActive, Guid ActionedBy);