using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Onefocus.Membership.Application.User.Commands;

public sealed record UpdatePasswordCommandRequest(Guid Id, string Password, string ConfirmPassword) : ICommand, IToObject<UpdatePasswordRepositoryRequest>
{
    public UpdatePasswordRepositoryRequest ToObject() => new (Id, Password);
}
internal sealed class UpdatePasswordCommandHandler : ICommandHandler<UpdatePasswordCommandRequest>
{
    private readonly IUserRepository _userRepository;

    public UpdatePasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return Result.Failure(validationResult.Error);
        
        return await _userRepository.UpdatePasswordAsync(request.ToObject());
    }

    private Result ValidateRequest(UpdatePasswordCommandRequest request)
    {
        if (request.Id == Guid.Empty) return Result.Failure(Errors.User.IdRequired);
        if (string.IsNullOrEmpty(request.Password)) return Result.Failure(Errors.User.PasswordRequired);
        if (string.IsNullOrEmpty(request.ConfirmPassword)) return Result.Failure(Errors.User.ConfirmPasswordRequired);
        if (request.Password.Equals(request.ConfirmPassword)) return Result.Failure(Errors.User.PasswordNotMatchConfirmPassword);

        return Result.Success();
    }
}

