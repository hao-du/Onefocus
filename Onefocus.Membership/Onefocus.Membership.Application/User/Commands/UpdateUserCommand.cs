using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.User.Services;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record UpdateUserCommand(Guid Id, string Email, string FirstName, string LastName) : ICommand, IRequestObject<UpdateUserServiceRequest>
{
    public UpdateUserServiceRequest ToRequestObject() => new (Id, Email, FirstName, LastName);
}
internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.UpdateUserAsync(request.ToRequestObject());
    }
}

