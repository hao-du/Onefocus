namespace Onefocus.Wallet.Application.Interfaces.ServiceBus;

public interface ISchemaPublisher
{
    Task PublishTransactionSchema();
}
