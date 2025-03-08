using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Configurations;
using Onefocus.Common.Results;
using Onefocus.Common.Security;
using Onefocus.Membership.Domain;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Onefocus.Membership.Infrastructure.ServiceBus;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipelines;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record UpdatePasswordCommandRequest(Guid Id, string Password, string ConfirmPassword) : ICommand
{
    public UpdatePasswordRepositoryRequest ConvertTo() => new (Id, Password);
}
internal sealed class UpdatePasswordCommandHandler : ICommandHandler<UpdatePasswordCommandRequest>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSyncedPublisher _userSyncedPublisher;
    private readonly IAuthenticationSettings? _authSettings;

    public UpdatePasswordCommandHandler(
        IUserRepository userRepository
        , IUserSyncedPublisher userSyncedPublisher
        , IOptions<AuthenticationSettings> options
    )
    {
        _userRepository = userRepository;
        _userSyncedPublisher = userSyncedPublisher;
        _authSettings = options.Value;
    }

    public async Task<Result> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return Result.Failure(validationResult.Error);
        
        var responseResult = await _userRepository.UpdatePasswordAsync(request.ConvertTo());
        if (responseResult.IsFailure) return Result.Failure(responseResult.Error);

        var encryptedPassword = Cryptography.Encrypt(request.Password, _authSettings?.SymmetricSecurityKeyString);

        var eventPublishResult = await _userSyncedPublisher.Publish(responseResult.Value.ToObject(encryptedPassword));
        return eventPublishResult;
    }

    private Result ValidateRequest(UpdatePasswordCommandRequest request)
    {
        if (request.Id == Guid.Empty) return Result.Failure(Errors.User.IdRequired);
        if (string.IsNullOrEmpty(request.Password)) return Result.Failure(Errors.User.PasswordRequired);
        if (string.IsNullOrEmpty(request.ConfirmPassword)) return Result.Failure(Errors.User.ConfirmPasswordRequired);
        if (!request.Password.Equals(request.ConfirmPassword)) return Result.Failure(Errors.User.PasswordNotMatchConfirmPassword);

        return Result.Success();
    }
}

