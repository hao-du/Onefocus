using Onefocus.Common.Results;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Interfaces.Services
{
    internal interface IBankService
    {
        Task<Result> HasDuplicatedBank(Guid id, string name, CancellationToken cancellationToken);
        Task PublishEvents(Entity.Bank bank, CancellationToken cancellationToken);
    }
}
