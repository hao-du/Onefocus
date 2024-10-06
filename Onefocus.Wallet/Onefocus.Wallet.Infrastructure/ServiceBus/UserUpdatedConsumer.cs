using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Infrastructure.Repositories.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Infrastructure.ServiceBus
{
    internal class UserUpdatedConsumer : IConsumer<IUserUpdatedMessage>
    {
        private readonly IUserWriteRepository _userRepository;
        private readonly ILogger<UserUpdatedConsumer> _logger;

        public UserUpdatedConsumer(
            IUserWriteRepository userRepository
            , ILogger<UserUpdatedConsumer> logger
        )
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IUserUpdatedMessage> context)
        {
            var result = await _userRepository.UpsertUserAsync(UpsertUserRepositoryRequest.Cast(context.Message));
            if (result.IsFailure)
            {
                _logger.LogError($"Cannot upsert user through message queue with [Code: {result.Error.Code} Error: {result.Error.Description}]" );
            }
        }
    }
}
