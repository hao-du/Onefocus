using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Onefocus.Membership.Infrastructure.ServiceBus;
using System.ComponentModel.DataAnnotations;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record UpdateUserCommandRequest(Guid Id, string Email, string FirstName, string LastName) : ICommand
{
    public UpdateUserRepositoryRequest ToObject() => new(Id, Email, FirstName, LastName);

    public IUserSyncedMessage ToObject(Guid? id = null) => new UserSyncedPublishMessage(id ?? Id, Email, FirstName, LastName, null, true, null);
}

internal sealed class UpdateUserCommandHandler(
    IUserRepository userRepository
        , IUserSyncedPublisher userSyncedPublisher) : ICommandHandler<UpdateUserCommandRequest>
{
    public async Task<Result> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return Result.Failure(validationResult.Error);

        var repoResult = await userRepository.UpdateUserAsync(request.ToObject());
        if (repoResult.IsFailure) return Result.Failure(repoResult.Error);

        var eventPublishResult = await userSyncedPublisher.Publish(request.ToObject(request.Id), cancellationToken);
        return eventPublishResult;
    }

    private static Result ValidateRequest(UpdateUserCommandRequest request)
    {
        if (request.Id == Guid.Empty) return Result.Failure(Errors.User.IdRequired);
        if (string.IsNullOrEmpty(request.FirstName)) return Result.Failure(Errors.User.FirstNameRequired);
        if (string.IsNullOrEmpty(request.LastName)) return Result.Failure(Errors.User.LastNameRequired);
        if (string.IsNullOrEmpty(request.Email)) return Result.Failure(Errors.User.EmailRequired);

        var emailAddressAttribute = new EmailAddressAttribute();
        if (!emailAddressAttribute.IsValid(request.Email)) return Result.Failure(Errors.User.InvalidEmail);

        return Result.Success();
    }
}

