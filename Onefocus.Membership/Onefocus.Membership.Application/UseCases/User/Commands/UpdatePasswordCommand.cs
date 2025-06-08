using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.Interfaces.Repositories;
using Onefocus.Membership.Domain;
using Entity = Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Application.UseCases.User.Commands;

public sealed record UpdatePasswordCommandRequest(Guid Id, string Password, string ConfirmPassword) : ICommand;
internal sealed class UpdatePasswordCommandHandler(
    IUserRepository userRepository
        , IPasswordHasher<Entity.User> passwordHasher
        , IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<UpdatePasswordCommandRequest>(httpContextAccessor)
{
    public override async Task<Result> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return validationResult;

        var userResult = await userRepository.GetUserByIdAsync(new(request.Id), cancellationToken);
        if (userResult.IsFailure) return userResult;
        if (userResult.Value.User is null) return Result.Failure(Errors.User.UserNotExist);

        var hashedPassword = passwordHasher.HashPassword(userResult.Value.User, request.Password);
        var updatePasswordResult = userResult.Value.User.Update(hashedPassword);
        if (updatePasswordResult.IsFailure) return updatePasswordResult;

        var updateUserResult = await userRepository.UpdateUserAsync(new(userResult.Value.User), cancellationToken);
        return updateUserResult;
    }

    private static Result ValidateRequest(UpdatePasswordCommandRequest request)
    {
        if (request.Id == Guid.Empty) return Result.Failure(Errors.User.IdRequired);
        if (string.IsNullOrEmpty(request.Password)) return Result.Failure(Errors.User.PasswordRequired);
        if (string.IsNullOrEmpty(request.ConfirmPassword)) return Result.Failure(Errors.User.ConfirmPasswordRequired);
        if (!request.Password.Equals(request.ConfirmPassword)) return Result.Failure(Errors.User.PasswordNotMatchConfirmPassword);

        return Result.Success();
    }
}

