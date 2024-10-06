using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using System.ComponentModel.DataAnnotations;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Membership.Infrastructure.ServiceBus;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record UpdateUserCommandRequest(Guid Id, string Email, string FirstName, string LastName) : ICommand, IToObject<UpdateUserRepositoryRequest>, IToObject<IUserUpdatedMessage, Guid?>
{
    public UpdateUserRepositoryRequest ToObject() => new (Id, Email, FirstName, LastName);

    public IUserUpdatedMessage ToObject(Guid? id = null)
    {
        throw new NotImplementedException();
    }
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

        return await _userRepository.UpdateUserAsync(request.ToObject());
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

