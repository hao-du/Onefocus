using Entity = Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Application.Contracts.Services.Token;

public sealed record GenerateTokenRequestDto(Entity.User User);