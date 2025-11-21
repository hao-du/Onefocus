using Onefocus.Common.Abstractions.ServiceBus.Search;

namespace Onefocus.Wallet.Application.Contracts.ServiceBus.Search;

public record SearchIndexMessage(IReadOnlyList<ISearchIndexDocument> Documents) : ISearchIndexMessage;