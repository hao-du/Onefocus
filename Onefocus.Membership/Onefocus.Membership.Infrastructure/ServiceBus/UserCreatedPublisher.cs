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
    public record UserCreatedPublishMessage(Guid Id, string Email, string FirstName, string LastName): IUserCreatedMessage;

    public interface IUserCreatedPublisher
    {
        Task<Result> Publish(IUserCreatedMessage message, CancellationToken cancellationToken = default);
    }

    public class UserCreatedPublisher: IUserCreatedPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<UserCreatedPublisher> _logger;

        public UserCreatedPublisher(
            IPublishEndpoint publishEndpoint
            , ILogger<UserCreatedPublisher> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task<Result> Publish(IUserCreatedMessage message, CancellationToken cancellationToken = default)
        {
            try
            {
                await _publishEndpoint.Publish(message, cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure(CommonErrors.InternalServer);
            }
        }
    }
}
