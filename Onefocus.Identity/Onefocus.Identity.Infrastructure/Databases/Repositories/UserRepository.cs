using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Common.Security;
using Onefocus.Identity.Domain;
using Onefocus.Identity.Domain.Entities;
using System.Linq;
using Entity = Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.Databases.Repositories;

public interface IUserRepository
{
    Task<Result<CheckPasswordRepositoryResponse>> CheckPasswordAsync(CheckPasswordRepositoryRequest request);
    Task<Result<GetUserByIdRepositoryResponse>> GetUserByIdAsync(GetUserByIdRepositoryRequest request);
    Task<Result> UpsertUserByNameAsync(UpsertUserRepositoryRequest request);
}

public sealed class UserRepository : IUserRepository
{
    private readonly UserManager<Entity.User> _userManager;
    private readonly IUserStore<Entity.User> _userStore;
    private readonly IUserEmailStore<Entity.User> _emailStore;
    private readonly IPasswordHasher<Entity.User> _passwordHasher;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(UserManager<Entity.User> userManager
        , IUserStore<Entity.User> userStore
        , ILogger<UserRepository> logger
        , IPasswordHasher<Entity.User> passwordHasher)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = (IUserEmailStore<Entity.User>)userStore;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<CheckPasswordRepositoryResponse>> CheckPasswordAsync(CheckPasswordRepositoryRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result.Failure<CheckPasswordRepositoryResponse>(Errors.User.IncorrectUserNameOrPassword);
        }

        bool passwordIsCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordIsCorrect)
        {
            return Result.Failure<CheckPasswordRepositoryResponse>(Errors.User.IncorrectUserNameOrPassword);
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Result.Success<CheckPasswordRepositoryResponse>(new(user, roles.ToList()));
    }

    public async Task<Result<GetUserByIdRepositoryResponse>> GetUserByIdAsync(GetUserByIdRepositoryRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return Result.Failure<GetUserByIdRepositoryResponse>(Errors.User.IncorrectUserNameOrPassword);
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Result.Success<GetUserByIdRepositoryResponse>(new(user, roles.ToList()));
    }

    public async Task<Result> UpsertUserByNameAsync(UpsertUserRepositoryRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Email);
        if (user == null)
        {
            return await CreateUserAsync(request);
        }
        else
        {
            return await UpdateUserAsync(user, request);
        }
    }

    private async Task<Result> CreateUserAsync(UpsertUserRepositoryRequest request)
    {
        try
        {
            var userResult = User.Create(request.Email);
            if (userResult.IsFailure)
            {
                return Result.Failure(userResult.Error);
            }

            var user = userResult.Value;

            await _userStore.SetUserNameAsync(user, request.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, request.Email, CancellationToken.None);

            IdentityResult identityResult;
            if (!string.IsNullOrEmpty(request.HashedPassword))
            {
                identityResult = await _userManager.CreateAsync(user, Cryptography.Decrypt(request.HashedPassword));
            }
            else 
            {
                identityResult = await _userManager.CreateAsync(user);
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<Guid>(CommonErrors.InternalServer);
        }
    }

    private async Task<Result> UpdateUserAsync(User user, UpsertUserRepositoryRequest request)
    {
        try
        {
            user.Update(request.Email);
            if (!string.IsNullOrEmpty(request.HashedPassword))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, Cryptography.Decrypt(request.HashedPassword));
            }

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Result.Failure(result.Errors.Select(e => new Error(e.Code, e.Description)).ToList());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure(CommonErrors.InternalServer);
        }

        return Result.Success();
    }
}