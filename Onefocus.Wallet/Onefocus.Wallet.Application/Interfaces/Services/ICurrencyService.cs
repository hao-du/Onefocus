using Onefocus.Common.Results;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Interfaces.Services
{
    internal interface ICurrencyService
    {
        Task<Result> HasDuplicatedCurrency(Guid id, string name, string shortName, CancellationToken cancellationToken);
        Task PublishEvents(Entity.Currency currency, CancellationToken cancellationToken);
    }
}
