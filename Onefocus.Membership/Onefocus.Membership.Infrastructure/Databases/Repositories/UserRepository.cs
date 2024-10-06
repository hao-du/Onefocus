using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using Onefocus.Membership.Domain.Entities;
using System.Linq;
using Entity = Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Infrastructure.Databases.Repositories;

public interface IUserRepository
{
    Task<Result<GetAllUsersRepositoryResponse>> GetAllUsersAsync();
    Task<Result<GetUserByIdRepositoryResponse>> GetUserByIdAsync(GetUserByIdRepositoryRequest request);
    Task<Result<Guid>> CreateUserAsync(CreateUserRepositoryRequest request);
    Task<Result> UpdateUserAsync(UpdateUserRepositoryRequest request);
    Task<Result> UpdatePasswordAsync(UpdatePasswordRepositoryRequest request);
}

public sealed class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly IUserStore<User> _userStore;
    private readonly IUserEmailStore<User> _emailStore;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(UserManager<User> userManager
        , IUserStore<User> userStore
        , ILogger<UserRepository> logger
        , IPasswordHasher<User> passwordHasher)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = (IUserEmailStore<User>)userStore;
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
                .Select(u => new GetAllUsersRepositoryResponse.UserReponse(
                    u.Id
                    , u.UserName
                    , u.Email
                    , u.FirstName
                    , u.LastName
                    , u.UserRoles.Select(c => GetAllUsersRepositoryResponse.RoleRepsonse.Cast(c.Role)).ToList())
                )
                .ToListAsync();

            return Result.Success(new GetAllUsersRepositoryResponse(users));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<GetAllUsersRepositoryResponse>(CommonErrors.InternalServer);
        }
    }

    public async Task<Result<GetUserByIdRepositoryResponse>> GetUserByIdAsync(GetUserByIdRepositoryRequest request)
    {
        try
        {
            var user = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user == null)
            {
                return Result.Failure<GetUserByIdRepositoryResponse>(Errors.User.UserNotExist);
            }

            return Result.Success(GetUserByIdRepositoryResponse.Cast(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<GetUserByIdRepositoryResponse>(CommonErrors.InternalServer);
        }
    }

    public async Task<Result<Guid>> CreateUserAsync(CreateUserRepositoryRequest request)
    {
        var userResult = User.Create(request.ToObject());
        if (userResult.IsFailure)
        {
            return Result.Failure<Guid>(userResult.Error);
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

    public async Task<Result> UpdateUserAsync(UpdateUserRepositoryRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return Result.Failure(Errors.User.UserNotExist);
        }

        user.Update(request.ToObject());

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

        var updatePasswordResult = user.Update(request.ToObject(), _passwordHasher);
        if (updatePasswordResult.IsFailure) return Result.Failure(updatePasswordResult.Error);

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