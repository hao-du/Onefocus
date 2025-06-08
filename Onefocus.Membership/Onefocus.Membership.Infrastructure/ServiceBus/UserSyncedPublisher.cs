using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Exceptions;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.Interfaces.ServiceBus;

namespace Onefocus.Membership.Infrastructure.ServiceBus
{
    public class UserSyncedPublisher(
        IPublishEndpoint publishEndpoint
        , ILogger<UserSyncedPublisher> logger) : IUserSyncedPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ILogger<UserSyncedPublisher> _logger = logger;

        public async Task<Result> Publish(IUserSyncedMessage message, CancellationToken cancellationToken = default)
        {
            try
            {
                await _publishEndpoint.Publish(message, cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot sync user {message.FirstName} {message.LastName} - email: {message.Email} - id: {message.Id} with error: {ex.Message}", message.FirstName, message.LastName, message.Email, message.Id, ex.Message);
                return Result.Failure(ex.ToErrors());
            }
        }
    }
}
