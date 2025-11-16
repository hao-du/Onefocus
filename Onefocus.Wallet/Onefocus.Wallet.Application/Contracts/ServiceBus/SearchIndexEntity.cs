using Onefocus.Common.Abstractions.ServiceBus.Search;

namespace Onefocus.Membership.Application.Contracts.ServiceBus;

public record SearchIndexEntity(string? IndexName, string? EntityId, string Payload) : ISearchIndexEntity;