using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Configurations;
using Onefocus.Common.Results;
using Onefocus.Common.Security;
using Onefocus.Membership.Domain;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Onefocus.Membership.Infrastructure.ServiceBus;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record UpdatePasswordCommandRequest(Guid Id, string Password, string ConfirmPassword) : ICommand
{
    public UpdatePasswordRepositoryRequest ToObject() => new(Id, Password);
}
internal sealed class UpdatePasswordCommandHandler(
    IUserRepository userRepository
        , IUserSyncedPublisher userSyncedPublisher
        , IAuthenticationSettings authSettings
    ) : ICommandHandler<UpdatePasswordCommandRequest>
{
    public async Task<Result> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return validationResult;

        var responseResult = await userRepository.UpdatePasswordAsync(request.ToObject());
        if (responseResult.IsFailure) return responseResult;

        var encryptedPassword = await Cryptography.Encrypt(request.Password, authSettings.SymmetricSecurityKey);

        var eventPublishResult = await userSyncedPublisher.Publish(responseResult.Value.ToObject(encryptedPassword), cancellationToken);
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

