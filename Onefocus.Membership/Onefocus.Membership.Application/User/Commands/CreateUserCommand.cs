using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.User.Services;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record CreateUserCommand(string Email, string FirstName, string LastName, string Password) : ICommand, IRequestObject<CreateUserServiceRequest>
{
    public CreateUserServiceRequest ToRequestObject() => new (Email, FirstName, LastName, Password);
}

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.CreateUserAsync(request.ToRequestObject());
    }
}

