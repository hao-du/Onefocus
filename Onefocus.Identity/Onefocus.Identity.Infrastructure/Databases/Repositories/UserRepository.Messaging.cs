using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.Databases.Repositories;

public sealed record CheckPasswordRepositoryRequest(string Email, string Password);
public sealed record CheckPasswordRepositoryResponse(User User, List<string> Roles);

public sealed record GetUserByIdRepositoryRequest(Guid Id);
public sealed record GetUserByIdRepositoryResponse(User User, List<string> Roles);

public sealed record UpsertUserRepositoryRequest(Guid Id, string Email, string? EncryptedPassword)
{
    public static UpsertUserRepositoryRequest CastFrom(IUserSyncedMessage source) => new(source.Id, source.Email, source.EncryptedPassword);
}