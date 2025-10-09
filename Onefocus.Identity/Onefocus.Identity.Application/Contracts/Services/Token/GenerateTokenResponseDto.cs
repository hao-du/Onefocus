namespace Onefocus.Identity.Application.Contracts.Services.Token;

public sealed record GenerateTokenResponseDto(string AccessToken, DateTime ExpiresAtUtc);