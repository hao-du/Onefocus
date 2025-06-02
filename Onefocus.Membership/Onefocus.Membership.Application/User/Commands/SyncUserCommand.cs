using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Onefocus.Membership.Infrastructure.ServiceBus;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record SyncUserCommandRequest() : ICommand;

internal sealed class SyncUserCommandHandler(
    IUserRepository userRepository
        , IUserSyncedPublisher userSyncedPublisher
        , ILogger<SyncUserCommandHandler> logger) : ICommandHandler<SyncUserCommandRequest>
{
    public async Task<Result> Handle(SyncUserCommandRequest request, CancellationToken cancellationToken)
    {
        var allUsersResult = await userRepository.GetAllUsersAsync();
        if (allUsersResult.IsFailure) return Result.Failure(allUsersResult.Error);
        if (allUsersResult.Value == null)
        {
            logger.LogInformation("There are no users to sync.");
            return Result.Success();
        }

        var tasks = new List<Task>();
        var errors = new List<Error>();
        foreach (var user in allUsersResult.Value.Users)
        {
            tasks.Add(Publish(user.ToObject(), errors));
        }
        await Task.WhenAll(tasks);

        if (errors.Count != 0)
        {
            return Result.Failure(errors);
        }
        return Result.Success();
    }

    public async Task Publish(IUserSyncedMessage message, List<Error> errors)
    {
        var eventResult = await userSyncedPublisher.Publish(message);
        if (eventResult.IsFailure)
        {
            errors.Add(eventResult.Error);
        }
    }
}

