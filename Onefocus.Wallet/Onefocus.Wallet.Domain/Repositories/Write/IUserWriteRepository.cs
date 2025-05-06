using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Write;

namespace Onefocus.Wallet.Domain.Repositories.Write;

public interface IUserWriteRepository
{
    Task<Result<UpsertUserResponse>> UpsertUserAsync(UpsertUserRequest request);
}