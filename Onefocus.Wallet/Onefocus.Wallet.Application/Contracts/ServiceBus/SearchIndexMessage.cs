using Onefocus.Common.Abstractions.ServiceBus.Search;

namespace Onefocus.Membership.Application.Contracts.ServiceBus;

public record SearchIndexMessage(IReadOnlyList<ISearchIndexEntity> Entities) : ISearchIndexMessage;