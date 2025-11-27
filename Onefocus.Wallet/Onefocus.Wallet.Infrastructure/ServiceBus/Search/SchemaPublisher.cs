using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Utilities;
using Onefocus.Wallet.Application.Contracts.ServiceBus.Search;
using Onefocus.Wallet.Application.Interfaces.ServiceBus;
using System.Reflection;

namespace Onefocus.Wallet.Infrastructure.ServiceBus.Search;

public class SchemaPublisher(
    IPublishEndpoint publishEndpoint,
    ILogger<SearchIndexPublisher> logger
) : ISchemaPublisher
{
    public Task PublishTransactionSchema()
    {
        return PublishSchema("transaction", "transaction.json");
    }

    private async Task PublishSchema(string indexName, string fileName)
    {
        logger.LogInformation("Publishing schema: {indexName}.", indexName);

        var rootFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (rootFolder == null)
        {
            logger.LogError("Root folder is null for {fileName}.", fileName);
            return;
        }
        var schemaPath = Path.Combine(rootFolder, "SearchSchemas", fileName);
        if (!File.Exists(schemaPath))
        {
            logger.LogError("{fileName} does not exist in {schemaPath}.", fileName, schemaPath);
            return;
        }
        string content = File.ReadAllText(schemaPath);
        if (!JsonHelper.IsValidJson(content))
        {
            logger.LogError("{fileName} has invalid json format in {schemaPath}.", fileName, schemaPath);
            return;
        }

        var schemaEvent = new SearchSchemaMessage
        (
            IndexName: indexName,
            Mappings: content
        );

        await publishEndpoint.Publish(schemaEvent);

        logger.LogInformation("Published schema: {indexName}.", indexName);
    }
}
