using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Configurations;
using Onefocus.Common.Results;
using Onefocus.Common.Security;
using Onefocus.Membership.Domain;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Onefocus.Membership.Infrastructure.ServiceBus;
using System.ComponentModel.DataAnnotations;

namespace Onefocus.Membership.Application.User.Commands;
public sealed record CreateUserCommandRequest(string Email, string FirstName, string LastName, string Password) : ICommand
{
    public CreateUserRepositoryRequest ToObject() => new(Email, FirstName, LastName, Password);
    public IUserSyncedMessage ToObject(Guid id, string encryptedPassword) => new UserSyncedPublishMessage(id, Email, FirstName, LastName, null, true, encryptedPassword);

}

internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository
        , IUserSyncedPublisher userSyncedPublisher
        , IAuthenticationSettings authenticationSettings
    ) : ICommandHandler<CreateUserCommandRequest>
{
    public async Task<Result> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return validationResult;

        var repoResult = await userRepository.CreateUserAsync(request.ToObject());
        if (repoResult.IsFailure) return repoResult.Error;

        var encryptedPassword = await Cryptography.Encrypt(request.Password, authenticationSettings.SymmetricSecurityKey);

        var eventPublishResult = await userSyncedPublisher.Publish(request.ToObject(repoResult.Value, encryptedPassword), cancellationToken);
        return eventPublishResult;
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

