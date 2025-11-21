using Onefocus.Common.Search.Schema;

namespace Onefocus.Wallet.Application.Interfaces.ServiceBus;

public interface ISchemaPublisher
{
    Task PublishTransactionSchema();
}
