using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Search.Schema;
using Onefocus.Wallet.Application.Contracts.ServiceBus.Search;
using Onefocus.Wallet.Application.Interfaces.ServiceBus;
using Onefocus.Wallet.Infrastructure.ServiceBus.Search.SchemaBuilders;

namespace Onefocus.Wallet.Infrastructure.ServiceBus.Search;

public class SchemaPublisher (
    IPublishEndpoint publishEndpoint,
    ILogger<SearchIndexPublisher> logger
) : ISchemaPublisher
{
    public Task PublishTransactionSchema()
    {
        var schema = TransactionSchemaBuilder.BuildMappingSchema();
        return PublishSchema(schema);
    }

    private async Task PublishSchema(MappingSchema schema)
    {
        var schemaEvent = new SearchSchemaMessage
        (
            EventId: Guid.NewGuid(),
            SchemaName: schema.SchemaName,
            IndexName: schema.IndexName,
            Schema: schema,
            Timestamp: DateTime.UtcNow,
            Metadata: new Dictionary<string, string>
            {
                ["TotalFields"] = schema.Fields.Count.ToString(),
                ["HasDynamicTemplates"] = (schema.DynamicTemplates?.Count > 0).ToString()
            }
        );

        await publishEndpoint.Publish(schemaEvent);

        logger.LogInformation("Published schema: {SchemaName}", schema.SchemaName);
    }
}
