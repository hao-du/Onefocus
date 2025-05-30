﻿using MassTransit;
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
    public record UserSyncedPublishMessage(Guid Id, string Email, string FirstName, string LastName, string? Description, bool IsActive, string? EncryptedPassword): IUserSyncedMessage;

    public interface IUserSyncedPublisher
    {
        Task<Result> Publish(IUserSyncedMessage message, CancellationToken cancellationToken = default);
    }

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
