using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using Onefocus.Membership.Infrastructure.Databases.Repositories.User;
using System.ComponentModel.DataAnnotations;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record CreateUserCommandRequest(string Email, string FirstName, string LastName, string Password) : ICommand<CreateUserCommandResponse>, IRequestObject<CreateUserRepositoryRequest>
{
    public CreateUserRepositoryRequest ToRequestObject() => new(Email, FirstName, LastName, Password);
}

public sealed record CreateUserCommandResponse(Guid Id) : IResponseObject<CreateUserCommandResponse, CreateUserRepositoryResponse>
{
    public static CreateUserCommandResponse Create(CreateUserRepositoryResponse source) => new CreateUserCommandResponse(source.Id);
}

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<CreateUserCommandResponse>> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var emailAddressAttribute = new EmailAddressAttribute();
        if (!emailAddressAttribute.IsValid(request.Email)) return Result.Failure<CreateUserCommandResponse>(Errors.User.InvalidEmail);
        if (string.IsNullOrEmpty(request.FirstName)) return Result.Failure<CreateUserCommandResponse>(Errors.User.FirstNameRequired);
        if (string.IsNullOrEmpty(request.LastName)) return Result.Failure<CreateUserCommandResponse>(Errors.User.LastNameRequired);
        if (string.IsNullOrEmpty(request.Email)) return Result.Failure<CreateUserCommandResponse>(Errors.User.EmailRequired);

        var createUserResult = await _userRepository.CreateUserAsync(request.ToRequestObject());
        if(createUserResult.IsFailure)
        {
            return Result.Failure<CreateUserCommandResponse>(createUserResult.Error);
        }

        return Result.Success<CreateUserCommandResponse>(CreateUserCommandResponse.Create(createUserResult.Value));
    }
}

