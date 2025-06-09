using Entity = Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Application.Contracts.Repositories.User;

public sealed record CreateUserRequestDto(Entity.User User, string? Password);