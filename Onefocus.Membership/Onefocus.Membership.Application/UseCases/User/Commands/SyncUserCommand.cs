using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.Contracts.ServiceBus;
using Onefocus.Membership.Application.Interfaces.Repositories;
using Onefocus.Membership.Application.Interfaces.ServiceBus;

namespace Onefocus.Membership.Application.UseCases.User.Commands;

public sealed record SyncUserCommandRequest() : ICommand;

internal sealed class SyncUserCommandHandler(
    IUserRepository userRepository
        , ISyncUserPublisher syncUserPublisher
        , IHttpContextAccessor httpContextAccessor
        , ILogger<SyncUserCommandHandler> logger) : CommandHandler<SyncUserCommandRequest>(httpContextAccessor, logger)
{
    public override async Task<Result> Handle(SyncUserCommandRequest request, CancellationToken cancellationToken)
    {
        var allUsersResult = await userRepository.GetAllUsersAsync(cancellationToken);
        if (allUsersResult.IsFailure) return allUsersResult;
        if (allUsersResult.Value.Users.Count <= 0)
        {
            logger.LogInformation("There are no users to sync.");
            return Result.Success();
        }

        var tasks = new List<Task>();
        var errors = new List<Error>();
        foreach (var user in allUsersResult.Value.Users)
        {
            tasks.Add(Publish(new SyncUserPublishMessage(
                Id: user.Id,
                Email: user.Email!,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Description: null,
                IsActive: true,
                EncryptedPassword: null
            ), errors, cancellationToken));
        }
        await Task.WhenAll(tasks);

        if (errors.Count != 0)
        {
            return Result.Failure(errors);
        }
        return Result.Success();
    }

    private async Task Publish(ISyncUserMessage message, List<Error> errors, CancellationToken cancellationToken)
    {
        var eventResult = await syncUserPublisher.Publish(message, cancellationToken);
        if (eventResult.IsFailure)
        {
            errors.Add(eventResult.Error);
        }
    }
}

