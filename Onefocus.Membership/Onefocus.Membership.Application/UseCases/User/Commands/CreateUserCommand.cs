using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Configurations;
using Onefocus.Common.Results;
using Onefocus.Common.Security;
using Onefocus.Membership.Application.Contracts.ServiceBus;
using Onefocus.Membership.Application.Interfaces.Repositories;
using Onefocus.Membership.Application.Interfaces.ServiceBus;
using Onefocus.Membership.Domain;
using System.ComponentModel.DataAnnotations;
using Entity = Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Application.UseCases.User.Commands;

public sealed record CreateUserCommandRequest(string Email, string FirstName, string LastName, string Password) : ICommand<CreateUserCommandResponse>;
public sealed record CreateUserCommandResponse(Guid Id);

internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository
        , IUserSyncedPublisher userSyncedPublisher
        , IAuthenticationSettings authenticationSettings
        , IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<CreateUserCommandRequest, CreateUserCommandResponse>(httpContextAccessor)
{
    public override async Task<Result<CreateUserCommandResponse>> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return Failure(validationResult);

        var createUserResult = Entity.User.Create(
            email: request.Email,
            firstName: request.FirstName,
            lastName: request.LastName
        );
        if (createUserResult.IsFailure) return Failure(createUserResult);

        var repoResult = await userRepository.CreateUserAsync(new(createUserResult.Value, request.Password), cancellationToken);
        if (repoResult.IsFailure) return Failure(repoResult);

        var eventPublishResult = await PublishUserCreationEvent(createUserResult.Value, request.Password, authenticationSettings.SymmetricSecurityKey, cancellationToken);
        if (eventPublishResult.IsFailure) return Failure(eventPublishResult);

        return Result.Success<CreateUserCommandResponse>(new(createUserResult.Value.Id));
    }

    private async Task<Result> PublishUserCreationEvent(Entity.User user, string password, string securityKey, CancellationToken cancellationToken = default)
    {
        var encryptedPassword = await Cryptography.Encrypt(password, securityKey);
        var eventPublishResult = await userSyncedPublisher.Publish(new UserSyncedPublishMessage(
            Id: user.Id,
            Email: user.Email!,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Description: null,
            IsActive: true,
            EncryptedPassword: encryptedPassword
        ), cancellationToken);
        if (eventPublishResult.IsFailure) return Failure(eventPublishResult);

        return Result.Success();
    }

    private static Result ValidateRequest(CreateUserCommandRequest request)
    {
        if (string.IsNullOrEmpty(request.FirstName)) return Result.Failure(Errors.User.FirstNameRequired);
        if (string.IsNullOrEmpty(request.LastName)) return Result.Failure(Errors.User.LastNameRequired);
        if (string.IsNullOrEmpty(request.Email)) return Result.Failure(Errors.User.EmailRequired);

        var emailAddressAttribute = new EmailAddressAttribute();
        if (!emailAddressAttribute.IsValid(request.Email)) return Result.Failure(Errors.User.InvalidEmail);

        return Result.Success();
    }
}

