using Entity = Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Application.Contracts.Repositories.Token;

public sealed record GenerateRefreshTokenRequestDto(Entity.User User);