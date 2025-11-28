using Onefocus.Wallet.Application.Interfaces.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.Services.Search;

namespace Onefocus.Wallet.Application.Services;

public class SearchSchemaManagementService(
    ISearchSchemaPublisher searchSchemaPublisher
) : ISearchSchemaManagementService
{
    public async Task InitializeSchemaAsync()
    {
        await Task.WhenAll(
            searchSchemaPublisher.PublishBankSchema(),
            searchSchemaPublisher.PublishCounterpartySchema(),
            searchSchemaPublisher.PublishCurrencySchema(),
            searchSchemaPublisher.PublishTransactionSchema()
        );
    }
}
