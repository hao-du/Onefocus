using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using System.ComponentModel.DataAnnotations;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Membership.Infrastructure.ServiceBus;
using System.IO.Pipelines;
using Onefocus.Common.Abstractions.Messages;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record UpdateUserCommandRequest(Guid Id, string Email, string FirstName, string LastName) : ICommand
{
    public UpdateUserRepositoryRequest ToObject() => new (Id, Email, FirstName, LastName);

    public IUserSyncedMessage ToObject(Guid? id = null) => new UserSyncedPublishMessage(id.HasValue ? id.Value : Id, Email, FirstName, LastName, null, true, null);
}

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommandRequest>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSyncedPublisher _userSyncedPublisher;

    public UpdateUserCommandHandler(
        IUserRepository userRepository
        , IUserSyncedPublisher userSyncedPublisher)
    {
        _userRepository = userRepository;
        _userSyncedPublisher = userSyncedPublisher;
    }

    public async Task<Result> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return Result.Failure(validationResult.Error);

        var repoResult = await _userRepository.UpdateUserAsync(request.ToObject());
        if (repoResult.IsFailure) return Result.Failure(repoResult.Error);

        var eventPublishResult = await _userSyncedPublisher.Publish(request.ToObject(request.Id));
        return eventPublishResult;
    }

    private Result ValidateRequest(UpdateUserCommandRequest request)
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

