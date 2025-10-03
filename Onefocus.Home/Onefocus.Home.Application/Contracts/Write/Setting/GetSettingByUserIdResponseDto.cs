using Entity = Onefocus.Home.Domain.Entities.Write;

namespace Onefocus.Home.Application.Contracts.Write.Setting;

public sealed record GetSettingByUserIdResponseDto(Entity.Setting? Setting);