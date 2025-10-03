using Entity = Onefocus.Home.Domain.Entities.Write;

namespace Onefocus.Home.Application.Contracts.Write.User;

public sealed record GetUserByIdResponseDto(Entity.User? User);
