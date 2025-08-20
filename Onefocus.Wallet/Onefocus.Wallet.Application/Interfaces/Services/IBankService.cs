using Onefocus.Common.Results;

namespace Onefocus.Wallet.Application.Interfaces.Services
{
    internal interface IBankService
    {
        Task<Result> HasDuplicatedBank(Guid id, string name, CancellationToken cancellationToken);
    }
}
