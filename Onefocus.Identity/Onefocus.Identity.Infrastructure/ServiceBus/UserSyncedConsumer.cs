using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Identity.Infrastructure.Databases.Repositories;

namespace Onefocus.Identity.Infrastructure.ServiceBus
{
    internal class UserSyncedConsumer(
        IUserRepository userRepository
            , ILogger<UserSyncedConsumer> logger
        ) : IConsumer<IUserSyncedMessage>
    {
        public async Task Consume(ConsumeContext<IUserSyncedMessage> context)
        {
            var result = await userRepository.UpsertUserByNameAsync(UpsertUserRepositoryRequest.CastFrom(context.Message));
            if (result.IsFailure)
            {
                logger.LogError("Cannot upsert user through message queue with [Code: {param1} Error: {param2}]", result.Error.Code, result.Error.Description);
            }
        }
    }
}
