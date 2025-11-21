using Onefocus.Wallet.Application.Interfaces.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.Services.Search;

namespace Onefocus.Wallet.Application.Services.Search;

public class SchemaManagementService(
    ISchemaPublisher schemaPublisher
) : ISchemaManagementService
{
    public async Task InitializeSchemaAsync()
    {
        await Task.WhenAll(
            schemaPublisher.PublishTransactionSchema()
        );
    }
}
