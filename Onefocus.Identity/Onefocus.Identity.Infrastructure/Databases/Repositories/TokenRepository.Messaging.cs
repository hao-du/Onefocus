using Onefocus.Common.Abstractions.Messaging;

namespace Onefocus.Membership.Infrastructure.Databases.Repositories;

public sealed record UpsertTokenRepositoryRequest(string Email);
public sealed record UpsertTokenRepositoryResponse(string RefreshToken);