using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Configurations;
using Onefocus.Common.Results;
using Onefocus.Common.Security;
using Onefocus.Membership.Application.Contracts.ServiceBus;
using Onefocus.Membership.Application.Interfaces.Repositories;
using Onefocus.Membership.Application.Interfaces.ServiceBus;
using Onefocus.Membership.Domain;
using Entity = Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Application.UseCases.User.Commands;

public sealed record UpdatePasswordCommandRequest(Guid Id, string Password, string ConfirmPassword) : ICommand;
internal sealed class UpdatePasswordCommandHandler(
    ILogger<UpdatePasswordCommandHandler> logger
        , IUserRepository userRepository
        , IUserSyncedPublisher userSyncedPublisher
        , IPasswordHasher<Entity.User> passwordHasher
        , IAuthenticationSettings authenticationSettings
        , IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<UpdatePasswordCommandRequest>(httpContextAccessor, logger)
{
    public override async Task<Result> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return validationResult;

        var userResult = await userRepository.GetUserByIdAsync(new(request.Id), cancellationToken);
        if (userResult.IsFailure) return userResult;
        if (userResult.Value.User is null) return Result.Failure(Errors.User.UserNotExist);

        var hashedPassword = passwordHasher.HashPassword(userResult.Value.User, request.Password);
        var updatePasswordResult = userResult.Value.User.Update(hashedPassword);
        if (updatePasswordResult.IsFailure) return updatePasswordResult;

        var updateUserResult = await userRepository.UpdateUserAsync(new(userResult.Value.User), cancellationToken);
        if (updateUserResult.IsFailure) return updateUserResult;

        var eventPublishResult = await PublishUserCreationEvent(userResult.Value.User, request.Password, authenticationSettings.SymmetricSecurityKey, cancellationToken);
        return eventPublishResult;
    }

    private async Task<Result> PublishUserCreationEvent(Entity.User user, string password, string securityKey, CancellationToken cancellationToken = default)
    {
        var encryptedPassword = await Cryptography.Encrypt(password, securityKey);
        var eventPublishResult = await userSyncedPublisher.Publish(new SyncUserPublishMessage(
            Id: user.Id,
            Email: user.Email!,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Description: null,
            IsActive: true,
            EncryptedPassword: encryptedPassword
        ), cancellationToken);

        return eventPublishResult;
    }

    private static Result ValidateRequest(UpdatePasswordCommandRequest request)
    {
        if (request.Id == Guid.Empty) return Result.Failure(Errors.User.IdRequired);
        if (string.IsNullOrEmpty(request.Password)) return Result.Failure(Errors.User.PasswordRequired);
        if (string.IsNullOrEmpty(request.ConfirmPassword)) return Result.Failure(Errors.User.ConfirmPasswordRequired);
        if (!request.Password.Equals(request.ConfirmPassword)) return Result.Failure(Errors.User.PasswordNotMatchConfirmPassword);

        return Result.Success();
    }
}

