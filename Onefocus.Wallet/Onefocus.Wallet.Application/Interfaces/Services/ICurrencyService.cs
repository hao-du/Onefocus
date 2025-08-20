using Onefocus.Common.Results;

namespace Onefocus.Wallet.Application.Interfaces.Services
{
    internal interface ICurrencyService
    {
        Task<Result> HasDuplicatedCurrency(Guid id, string name, string shortName, CancellationToken cancellationToken);
    }
}
