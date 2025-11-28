namespace Onefocus.Wallet.Application.Interfaces.ServiceBus;

public interface ISearchSchemaPublisher
{
    Task PublishTransactionSchema();
    Task PublishBankSchema();
    Task PublishCurrencySchema();
    Task PublishCounterpartySchema();
}
