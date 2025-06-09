using Entity = Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Application.Contracts.Repositories.User;

public sealed record GetUserByEmailResponseDto(Entity.User? User);