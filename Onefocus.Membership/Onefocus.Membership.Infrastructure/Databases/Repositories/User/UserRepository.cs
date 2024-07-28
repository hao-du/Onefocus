using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using Entity = Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Infrastructure.Databases.Repositories.User;

public interface IUserRepository
{
    Task<Result<GetAllUsersRepositoryResponse>> GetAllUsersAsync();
    Task<Result> CreateUserAsync(CreateUserRepositoryRequest request);
    Task<Result> UpdateUserAsync(UpdateUserRepositoryRequest request);
    Task<Result> UpdatePasswordAsync(UpdatePasswordRepositoryRequest request);
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

    public async Task<Result<GetAllUsersRepositoryResponse>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => new GetAllUsersUserItemRepositoryResponse(u.Id, u.UserName, u.Email, u.FirstName, u.LastName))
                .ToListAsync();

            return Result.Success<GetAllUsersRepositoryResponse>(new GetAllUsersRepositoryResponse(users));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<GetAllUsersRepositoryResponse>(CommonErrors.InternalServer);
        }
    }

    public async Task<Result> CreateUserAsync(CreateUserRepositoryRequest request)
    {
        var userResult = Entity.User.Create(request.ToRequestObject());
        if (userResult.IsFailure)
        {
            return userResult;
        }

        var user = userResult.Value;
        try
        {
            await _userStore.SetUserNameAsync(user, request.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, request.Email, CancellationToken.None);

            var identityResult = await _userManager.CreateAsync(user, request.Password);
            if (!identityResult.Succeeded)
            {
                var identityError = identityResult.Errors.FirstOrDefault();
                if (identityError != null)
                {
                    return Result.Failure(new Error(identityError.Code, identityError.Description));
                }
                return Result.Failure(CommonErrors.Unknown);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure(CommonErrors.InternalServer);
        }

        return Result.Success();
    }

    public async Task<Result> UpdateUserAsync(UpdateUserRepositoryRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if(user == null)
        {
            return Result.Failure(Errors.User.UserNotExist);
        }

        user.Update(request.ToRequestObject());

        try
        {
            IdentityResult result = await _userManager.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure(CommonErrors.InternalServer);
        }

        return Result.Success();
    }

    public async Task<Result> UpdatePasswordAsync(UpdatePasswordRepositoryRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null) return Result.Failure(Errors.User.UserNotExist);

        var updatePasswordResult = user.Update(request.ToRequestObject(), _passwordHasher);
        if(updatePasswordResult.IsFailure) return Result.Failure(updatePasswordResult.Error);

        try
        {
            IdentityResult result = await _userManager.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure(CommonErrors.InternalServer);
        }

        return Result.Success();
    }
}