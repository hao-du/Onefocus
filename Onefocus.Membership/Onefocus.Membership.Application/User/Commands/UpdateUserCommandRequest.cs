using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using System.ComponentModel.DataAnnotations;
using Onefocus.Membership.Infrastructure.Databases.Repositories.User;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record UpdateUserCommandRequest(Guid Id, string Email, string FirstName, string LastName) : ICommand, IRequestObject<UpdateUserRepositoryRequest>
{
    public UpdateUserRepositoryRequest ToRequestObject() => new (Id, Email, FirstName, LastName);
}

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommandRequest>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return Result.Failure(validationResult.Error);

        return await _userRepository.UpdateUserAsync(request.ToRequestObject());
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

