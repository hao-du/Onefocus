using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.Contracts.ServiceBus;
using Onefocus.Membership.Application.Interfaces.Repositories;
using Onefocus.Membership.Application.Interfaces.ServiceBus;
using Onefocus.Membership.Domain;
using System.ComponentModel.DataAnnotations;
using Entity = Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Application.UseCases.User.Commands;

public sealed record UpdateUserCommandRequest(Guid Id, string Email, string FirstName, string LastName) : ICommand;

internal sealed class UpdateUserCommandHandler(
    IUserRepository userRepository
    , IUserSyncedPublisher userSyncedPublisher
    , IHttpContextAccessor httpContextAccessor
) : CommandHandler<UpdateUserCommandRequest>(httpContextAccessor)
{
    public override async Task<Result> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return validationResult;

        var userResult = await userRepository.GetUserByIdAsync(new(request.Id), cancellationToken);
        if (userResult.IsFailure) return userResult;
        if (userResult.Value.User is null) return Result.Failure(Errors.User.UserNotExist);

        var updateUserResult = await userRepository.UpdateUserAsync(new(userResult.Value.User), cancellationToken);
        if (updateUserResult.IsFailure) return updateUserResult;

        var eventPublishResult = await PublishUserUpdateEvent(userResult.Value.User, cancellationToken);
        return eventPublishResult;
    }

    private async Task<Result> PublishUserUpdateEvent(Entity.User user, CancellationToken cancellationToken = default)
    {
        var eventPublishResult = await userSyncedPublisher.Publish(new UserSyncedPublishMessage(
            Id: user.Id,
            Email: user.Email!,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Description: null,
            IsActive: true,
            EncryptedPassword: null
        ), cancellationToken);

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

