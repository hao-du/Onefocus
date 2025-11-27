using Onefocus.Common.Abstractions.ServiceBus.Search;

namespace Onefocus.Wallet.Application.Contracts.ServiceBus.Search;

public record SearchSchemaMessage(string IndexName, string Mappings) : ISearchSchemaMessage;