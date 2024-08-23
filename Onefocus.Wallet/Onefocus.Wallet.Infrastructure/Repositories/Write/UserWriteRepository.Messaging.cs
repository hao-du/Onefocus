using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Repositories.Write;

public sealed record UpsertUserRepositoryRequest(Guid Id, string Email, string FirstName, string LastName, string Description, bool ActionFlag, Guid ActionBy) : IRequestObject<Result<User>>
{
    public Result<User> ToRequestObject() => User.Create(Email, FirstName, LastName, Description, ActionBy);
}
public sealed record UpsertUserRepositoryResponse(Guid Id);

