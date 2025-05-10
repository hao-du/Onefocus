using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Messages.Write;

public sealed record UpsertUserRequestDto(Guid Id, string Email, string FirstName, string LastName, string? Description, bool ActionFlag, Guid ActionBy)
{
    public static UpsertUserRequestDto CastFrom(IUserSyncedMessage source) => new(source.Id, source.Email, source.FirstName, source.LastName, source.Description, source.ActionFlag, Guid.Empty);

    public Result<User> ToObject() => User.Create(Id, Email, FirstName, LastName, Description, ActionBy);

}
