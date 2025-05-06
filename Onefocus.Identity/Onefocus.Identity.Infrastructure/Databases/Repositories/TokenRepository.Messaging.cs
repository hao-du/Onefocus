using Onefocus.Identity.Domain.Entities;

namespace Onefocus.Membership.Infrastructure.Databases.Repositories;

public sealed record GenerateRefreshTokenRepositoryRequest(User User);
public sealed record GenerateRefreshTokenRepositoryResponse(string RefreshToken);

public sealed record MatchRefreshTokenRepositoryRequest(User User, string RefreshToken);