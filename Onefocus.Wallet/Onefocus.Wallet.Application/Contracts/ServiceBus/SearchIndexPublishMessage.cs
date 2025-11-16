using Onefocus.Common.Abstractions.ServiceBus.Membership;

namespace Onefocus.Membership.Application.Contracts.ServiceBus;

public record SearchIndexPublishMessage(string? EntityType, string? EntityId, string Payload) : ISearchIndexMessage;