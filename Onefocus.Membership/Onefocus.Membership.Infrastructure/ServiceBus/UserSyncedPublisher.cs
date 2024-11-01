using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Membership.Infrastructure.ServiceBus
{
    public record UserSyncedPublishMessage(Guid Id, string Email, string FirstName, string LastName, string? Description, bool ActionFlag, string? HashedPassword): IUserSyncedMessage;

    public interface IUserSyncedPublisher
    {
        Task<Result> Publish(IUserSyncedMessage message, CancellationToken cancellationToken = default);
    }

    public class UserSyncedPublisher : IUserSyncedPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<UserSyncedPublisher> _logger;

        public UserSyncedPublisher(
            IPublishEndpoint publishEndpoint
            , ILogger<UserSyncedPublisher> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task<Result> Publish(IUserSyncedMessage message, CancellationToken cancellationToken = default)
        {
            try
            {
                await _publishEndpoint.Publish(message, cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cannot sync user {message.FirstName} {message.LastName} - email: {message.Email} - id: {message.Id} with error: {ex.Message}");
                return Result.Failure(CommonErrors.InternalServer);
            }
        }
    }
}
