﻿using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Write;
using Onefocus.Wallet.Domain.Repositories.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Infrastructure.ServiceBus
{
    internal class UserSyncedConsumer : IConsumer<IUserSyncedMessage>
    {
        private readonly IUserWriteRepository _userRepository;
        private readonly ILogger<UserSyncedConsumer> _logger;

        public UserSyncedConsumer(
            IUserWriteRepository userRepository
            , ILogger<UserSyncedConsumer> logger
        )
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IUserSyncedMessage> context)
        {
            var result = await _userRepository.UpsertUserAsync(UpsertUserRequestDto.CastFrom(context.Message));
            if (result.IsFailure)
            {
                _logger.LogError($"Cannot upsert user through message queue with [Code: {result.Error.Code} Error: {result.Error.Description}]" );
            }
        }
    }
}
