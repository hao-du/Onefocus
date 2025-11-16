using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Exceptions;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.ServiceBus;

namespace Onefocus.Wallet.Infrastructure.ServiceBus
{
    public class SearchIndexPublisher(
        IPublishEndpoint publishEndpoint
        , ILogger<SearchIndexPublisher> logger) : ISearchIndexPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ILogger<SearchIndexPublisher> _logger = logger;

        public async Task<Result> Publish(ISearchIndexMessage message, CancellationToken cancellationToken = default)
        {
            try
            {
                await _publishEndpoint.Publish(message, cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot index {message.EntityType} - {message.EntityId} with error: {ex.Message}", message.EntityType, message.EntityId, ex.Message);
                return Result.Failure(ex.ToErrors());
            }
        }
    }
}
