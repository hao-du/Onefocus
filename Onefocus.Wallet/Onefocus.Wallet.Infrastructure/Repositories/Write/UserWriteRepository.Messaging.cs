using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Repositories.Write;

public sealed record UpsertUserRepositoryRequest(Guid Id, string Email, string FirstName, string LastName, string? Description, bool ActionFlag, Guid ActionBy) : IToObject<Result<User>>, ICastObject<UpsertUserRepositoryRequest, IUserCreatedMessage>, ICastObject<UpsertUserRepositoryRequest, IUserUpdatedMessage>
{
    public static UpsertUserRepositoryRequest Cast(IUserCreatedMessage source) => new(source.Id, source.Email, source.FirstName, source.LastName, null, true, Guid.Empty);

    public static UpsertUserRepositoryRequest Cast(IUserUpdatedMessage source) => new(source.Id, source.Email, source.FirstName, source.LastName, source.Description, source.ActionFlag, Guid.Empty);

    public Result<User> ToObject() => User.Create(Email, FirstName, LastName, Description, ActionBy);

}
public sealed record UpsertUserRepositoryResponse(Guid Id);

