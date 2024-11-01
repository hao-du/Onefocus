using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Identity.Infrastructure.Databases.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Identity.Infrastructure.ServiceBus
{
    internal class UserSyncedConsumer : IConsumer<IUserSyncedMessage>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserSyncedConsumer> _logger;

        public UserSyncedConsumer(
            IUserRepository userRepository
            , ILogger<UserSyncedConsumer> logger
        )
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IUserSyncedMessage> context)
        {
            var result = await _userRepository.UpsertUserByIdAsync(UpsertUserRepositoryRequest.Cast(context.Message));
            if (result.IsFailure)
            {
                _logger.LogError($"Cannot upsert user through message queue with [Code: {result.Error.Code} Error: {result.Error.Description}]");
            }
        }
    }
}
