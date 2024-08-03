using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Onefocus.Membership.Application.User.Commands;
public sealed record CreateUserCommandRequest(string Email, string FirstName, string LastName, string Password) : ICommand, IRequestObject<CreateUserRepositoryRequest>
{
    public CreateUserRepositoryRequest ToRequestObject() => new (Email, FirstName, LastName, Password);
}

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommandRequest>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return Result.Failure(validationResult.Error);

        return await _userRepository.CreateUserAsync(request.ToRequestObject());
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

