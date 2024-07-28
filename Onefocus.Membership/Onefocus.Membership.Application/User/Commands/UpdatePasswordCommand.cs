using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.User.Services;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record UpdatePasswordCommand(Guid Id, string Password, string ConfirmPassword) : ICommand, IRequestObject<UpdatePasswordServiceRequest>
{
    public UpdatePasswordServiceRequest ToRequestObject() => new (Id, Password, ConfirmPassword);
}
internal sealed class UpdatePasswordCommandHandler : ICommandHandler<UpdatePasswordCommand>
{
    private readonly IUserService _userService;

    public UpdatePasswordCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        return await _userService.UpdatePasswordAsync(request.ToRequestObject());
    }
}

