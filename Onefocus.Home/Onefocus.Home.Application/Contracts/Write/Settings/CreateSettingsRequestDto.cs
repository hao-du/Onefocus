using Entity = Onefocus.Home.Domain.Entities.Write;

namespace Onefocus.Home.Application.Contracts.Write.Settings
{
    public sealed record CreateSettingsRequestDto(Entity.Settings Settings);
}