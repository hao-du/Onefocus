using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Results;
using Onefocus.Identity.Application.Contracts.Repositories.User;
using Onefocus.Identity.Application.Interfaces.Repositories;
using DomainErrors = Onefocus.Identity.Domain.Errors;
using Entity = Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.Databases.Repositories;

public sealed class UserRepository(UserManager<Entity.User> userManager
        , IUserStore<Entity.User> userStore
        , ILogger<UserRepository> logger) : BaseIdentityRepository<UserRepository>(logger), IUserRepository
{
    private readonly IUserEmailStore<Entity.User> _emailStore = (IUserEmailStore<Entity.User>)userStore;

    public async Task<Result<CheckPasswordResponseDto>> CheckPasswordAsync(CheckPasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync<CheckPasswordResponseDto>(async () =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure<CheckPasswordResponseDto>(DomainErrors.User.IncorrectUserNameOrPassword);
            }

            bool passwordIsCorrect = await userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordIsCorrect)
            {
                return Result.Failure<CheckPasswordResponseDto>(DomainErrors.User.IncorrectUserNameOrPassword);
            }

            return Result.Success<CheckPasswordResponseDto>(new(user));
        });
    }

    public async Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync<GetUserByIdResponseDto>(async () =>
        {
            var user = await userManager.FindByIdAsync(request.Id.ToString());

            return Result.Success<GetUserByIdResponseDto>(new(user));
        });
    }

    public async Task<Result<GetUserByEmailResponseDto>> GetUserByEmailAsync(GetUserByEmailRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync<GetUserByEmailResponseDto>(async () =>
        {
            var user = await userManager.FindByNameAsync(request.Email);

            return Result.Success<GetUserByEmailResponseDto>(new(user));
        });
    }

    public async Task<Result> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var user = request.User;

            await userStore.SetUserNameAsync(user, user.Email, cancellationToken);
            await _emailStore.SetEmailAsync(user, user.Email, cancellationToken);

            IdentityResult identityResult;
            if (!string.IsNullOrEmpty(request.Password))
            {
                identityResult = await userManager.CreateAsync(user, request.Password);
            }
            else
            {
                identityResult = await userManager.CreateAsync(user);
            }
            if (!identityResult.Succeeded)
                return GetIdentityErrorResult(identityResult);

            return Result.Success();
        });
    }

    public async Task<Result> UpdateUserAsync(UpdateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var identityResult = await userManager.UpdateAsync(request.User);
            if (!identityResult.Succeeded)
                return GetIdentityErrorResult(identityResult);

            return Result.Success();
        });
    }
}