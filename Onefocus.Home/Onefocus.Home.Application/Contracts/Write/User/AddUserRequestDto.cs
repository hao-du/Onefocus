using Entity = Onefocus.Home.Domain.Entities.Write;

namespace Onefocus.Home.Application.Contracts.Write.User;

public sealed record AddUserRequestDto(Entity.User User);
