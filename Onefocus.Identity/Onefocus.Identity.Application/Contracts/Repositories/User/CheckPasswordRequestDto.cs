namespace Onefocus.Identity.Application.Contracts.Repositories.User;

public sealed record CheckPasswordRequestDto(string Email, string Password);