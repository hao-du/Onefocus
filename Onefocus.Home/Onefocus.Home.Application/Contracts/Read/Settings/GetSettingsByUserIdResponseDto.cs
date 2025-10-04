using Entity = Onefocus.Home.Domain.Entities.Read;

namespace Onefocus.Home.Application.Contracts.Read.Settings;

public sealed record GetSettingsByUserIdResponseDto(Entity.Settings? Settings);
