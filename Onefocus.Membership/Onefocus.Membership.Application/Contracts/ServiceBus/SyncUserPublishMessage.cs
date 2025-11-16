using Onefocus.Common.Abstractions.ServiceBus.Membership;

namespace Onefocus.Membership.Application.Contracts.ServiceBus;

public record SyncUserPublishMessage(Guid Id, string Email, string FirstName, string LastName, string? Description, bool IsActive, string? EncryptedPassword) : ISyncUserMessage;
