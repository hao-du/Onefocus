using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using System.ComponentModel.DataAnnotations;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Membership.Infrastructure.ServiceBus;
using System.IO.Pipelines;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record SyncUserCommandRequest() : ICommand;

internal sealed class SyncUserCommandHandler : ICommandHandler<SyncUserCommandRequest>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSyncedPublisher _userSyncedPublisher;
    private readonly ILogger<SyncUserCommandHandler> _logger;

    public SyncUserCommandHandler(
        IUserRepository userRepository
        , IUserSyncedPublisher userSyncedPublisher
        , ILogger<SyncUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _userSyncedPublisher = userSyncedPublisher;
        _logger = logger;
    }

    public async Task<Result> Handle(SyncUserCommandRequest request, CancellationToken cancellationToken)
    {
        var allUsersResult = await _userRepository.GetAllUsersAsync();
        if (allUsersResult.IsFailure) return Result.Failure(allUsersResult.Error);
        if (allUsersResult.Value == null)
        {
            _logger.LogInformation("There are no users to sync.");
            return Result.Success();
        }

        var tasks = new List<Task>();
        var errors = new List<Error>();
        foreach (var user in allUsersResult.Value.Users)
        {
            tasks.Add(Publish(user.ToObject(), errors));
        }
        await Task.WhenAll(tasks);

        if (errors.Any())
        {
            return Result.Failure(errors);
        }
        return Result.Success();
    }

    public async Task Publish(IUserSyncedMessage message, List<Error> errors)
    {
        var eventResult = await _userSyncedPublisher.Publish(message);
        if (eventResult.IsFailure)
        {
            errors.Add(eventResult.Error);
        }
    }
}

