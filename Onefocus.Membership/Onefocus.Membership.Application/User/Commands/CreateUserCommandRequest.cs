using MassTransit;
using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Onefocus.Membership.Infrastructure.ServiceBus;
using System.ComponentModel.DataAnnotations;

namespace Onefocus.Membership.Application.User.Commands;
public sealed record CreateUserCommandRequest(string Email, string FirstName, string LastName, string Password) : ICommand, IToObject<CreateUserRepositoryRequest>, IToObject<IUserCreatedMessage, Guid>
{
    public CreateUserRepositoryRequest ToObject() => new (Email, FirstName, LastName, Password);
    public IUserCreatedMessage ToObject(Guid id) => new UserCreatedPublishMessage(id, Email, FirstName, LastName);

}

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommandRequest>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserCreatedPublisher _userCreatedPublisher;

    public CreateUserCommandHandler(
        IUserRepository userRepository
        , IUserCreatedPublisher userCreatedPublisher
    )
    {
        _userRepository = userRepository;
        _userCreatedPublisher = userCreatedPublisher;
    }

    public async Task<Result> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return Result.Failure(validationResult.Error);

        var repoResult = await _userRepository.CreateUserAsync(request.ToObject());
        if (repoResult.IsFailure) return Result.Failure(validationResult.Error);

        var eventPublishResult = await _userCreatedPublisher.Publish(request.ToObject(repoResult.Value));

        return eventPublishResult;
    }

    private Result ValidateRequest(CreateUserCommandRequest request)
    {
        if (string.IsNullOrEmpty(request.FirstName)) return Result.Failure(Errors.User.FirstNameRequired);
        if (string.IsNullOrEmpty(request.LastName)) return Result.Failure(Errors.User.LastNameRequired);
        if (string.IsNullOrEmpty(request.Email)) return Result.Failure(Errors.User.EmailRequired);

        var emailAddressAttribute = new EmailAddressAttribute();
        if (!emailAddressAttribute.IsValid(request.Email)) return Result.Failure(Errors.User.InvalidEmail);

        return Result.Success();
    }
}

