using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Configurations;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Common.Security;
using Onefocus.Identity.Domain;
using Onefocus.Identity.Domain.Entities;
using Entity = Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.Databases.Repositories;

public interface IUserRepository
{
    Task<Result<CheckPasswordRepositoryResponse>> CheckPasswordAsync(CheckPasswordRepositoryRequest request);
    Task<Result<GetUserByIdRepositoryResponse>> GetUserByIdAsync(GetUserByIdRepositoryRequest request);
    Task<Result> UpsertUserByNameAsync(UpsertUserRepositoryRequest request);
}

public sealed class UserRepository(UserManager<Entity.User> userManager
        , IUserStore<Entity.User> userStore
        , ILogger<UserRepository> logger
        , IPasswordHasher<Entity.User> passwordHasher
        , IAuthenticationSettings authenticationSettings) : BaseRepository<UserRepository>(logger), IUserRepository
{
    private readonly IUserEmailStore<Entity.User> _emailStore = (IUserEmailStore<Entity.User>)userStore;

    public async Task<Result<CheckPasswordRepositoryResponse>> CheckPasswordAsync(CheckPasswordRepositoryRequest request)
    {
        return await ExecuteAsync<CheckPasswordRepositoryResponse>(async () =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure<CheckPasswordRepositoryResponse>(Errors.User.IncorrectUserNameOrPassword);
            }

            bool passwordIsCorrect = await userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordIsCorrect)
            {
                return Result.Failure<CheckPasswordRepositoryResponse>(Errors.User.IncorrectUserNameOrPassword);
            }

            var roles = await userManager.GetRolesAsync(user);

            return Result.Success<CheckPasswordRepositoryResponse>(new(user, [.. roles]));
        });
    }

    public async Task<Result<GetUserByIdRepositoryResponse>> GetUserByIdAsync(GetUserByIdRepositoryRequest request)
    {
        return await ExecuteAsync<GetUserByIdRepositoryResponse>(async () =>
        {
            var user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return Result.Failure<GetUserByIdRepositoryResponse>(Errors.User.IncorrectUserNameOrPassword);
            }

            var roles = await userManager.GetRolesAsync(user);

            return Result.Success<GetUserByIdRepositoryResponse>(new(user, [.. roles]));
        });
    }

    public async Task<Result> UpsertUserByNameAsync(UpsertUserRepositoryRequest request)
    {
        return await ExecuteAsync(async () =>
        {
            var user = await userManager.FindByNameAsync(request.Email);
            if (user == null)
            {
                return await CreateUserAsync(request);
            }
            else
            {
                return await UpdateUserAsync(user, request);
            }
        });
    }

    private async Task<Result> CreateUserAsync(UpsertUserRepositoryRequest request)
    {
        return await ExecuteAsync(async () =>
        {
            var userResult = User.Create(request.Email, request.Id);
            if (userResult.IsFailure)
            {
                return Result.Failure(userResult.Error);
            }

            var user = userResult.Value;

            await userStore.SetUserNameAsync(user, request.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, request.Email, CancellationToken.None);

            IdentityResult identityResult;
            if (!string.IsNullOrEmpty(request.EncryptedPassword))
            {
                var password = await Cryptography.Decrypt(request.EncryptedPassword, authenticationSettings.SymmetricSecurityKey);
                identityResult = await userManager.CreateAsync(user, password);
            }
            else
            {
                identityResult = await userManager.CreateAsync(user);
            }
            if (!identityResult.Succeeded)
            {
                var identityError = identityResult.Errors.FirstOrDefault();
                if (identityError != null)
                {
                    return Result.Failure<Guid>(new Error(identityError.Code, identityError.Description));
                }
                return Result.Failure<Guid>(CommonErrors.Unknown);
            }

            return Result.Success<Guid>(user.Id);
        });
    }

    private async Task<Result> UpdateUserAsync(User user, UpsertUserRepositoryRequest request)
    {
        return await ExecuteAsync(async () =>
        {
            user.Update(request.Email);
            if (!string.IsNullOrEmpty(request.EncryptedPassword))
            {
                var password = await Cryptography.Decrypt(request.EncryptedPassword, authenticationSettings.SymmetricSecurityKey);
                user.PasswordHash = passwordHasher.HashPassword(user, password);
            }

            IdentityResult result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Result.Failure([.. result.Errors.Select(e => new Error(e.Code, e.Description))]);
            }

            return Result.Success();
        });
    }
}