using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Common.Search.Schema;

namespace Onefocus.Wallet.Application.Contracts.ServiceBus.Search;

public record SearchSchemaMessage(Guid EventId, string SchemaName, string IndexName, MappingSchema Schema, DateTime Timestamp, Dictionary<string, string> Metadata) : ISearchSchemaMessage;