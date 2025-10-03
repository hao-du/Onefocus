using Entity = Onefocus.Home.Domain.Entities.Read;

namespace Onefocus.Home.Application.Contracts.Read.Setting;

public sealed record GetSettingByUserIdResponseDto(Entity.Setting? Setting);
