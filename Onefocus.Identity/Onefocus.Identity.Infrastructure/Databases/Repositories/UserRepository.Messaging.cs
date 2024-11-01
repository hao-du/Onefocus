using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Onefocus.Identity.Domain.Entities;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Entity = Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.Databases.Repositories;

public sealed record CheckPasswordRepositoryRequest(string Email, string Password);
public sealed record CheckPasswordRepositoryResponse(User User, List<string> Roles);

public sealed record GetUserByIdRepositoryRequest(Guid Id);
public sealed record GetUserByIdRepositoryResponse(User User, List<string> Roles);

public sealed record UpsertUserRepositoryRequest(Guid Id, string Email, string? HashedPassword)
{
    public static UpsertUserRepositoryRequest Cast(IUserSyncedMessage source) => new(source.Id, source.Email, source.HashedPassword);
}