using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Repositories.Write;

public sealed record UpsertUserRepositoryRequest(Guid Id, string Email, string FirstName, string LastName, string? Description, bool ActionFlag, Guid ActionBy)
{
    public static UpsertUserRepositoryRequest CastFrom(IUserSyncedMessage source) => new(source.Id, source.Email, source.FirstName, source.LastName, source.Description, source.ActionFlag, Guid.Empty);

    public Result<User> ConvertTo() => User.Create(Email, FirstName, LastName, Description, ActionBy);

}
public sealed record UpsertUserRepositoryResponse(Guid Id);

