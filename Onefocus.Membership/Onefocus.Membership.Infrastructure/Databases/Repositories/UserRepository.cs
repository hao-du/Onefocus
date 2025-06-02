using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Infrastructure.Databases.Repositories;

public interface IUserRepository
{
    Task<Result<GetAllUsersRepositoryResponse>> GetAllUsersAsync();
    Task<Result<GetUserByIdRepositoryResponse>> GetUserByIdAsync(GetUserByIdRepositoryRequest request);
    Task<Result<Guid>> CreateUserAsync(CreateUserRepositoryRequest request);
    Task<Result> UpdateUserAsync(UpdateUserRepositoryRequest request);
    Task<Result<UpdatePasswordRepositoryResponse>> UpdatePasswordAsync(UpdatePasswordRepositoryRequest request);
}

public sealed class UserRepository(UserManager<User> userManager
        , IUserStore<User> userStore
        , ILogger<UserRepository> logger
        , IPasswordHasher<User> passwordHasher) : BaseRepository<UserRepository>(logger), IUserRepository
{
    private readonly IUserEmailStore<User> _emailStore = (IUserEmailStore<User>)userStore;

    public async Task<Result<GetAllUsersRepositoryResponse>> GetAllUsersAsync()
    {
        return await ExecuteAsync<GetAllUsersRepositoryResponse>(async () =>
        {
            var users = await userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => new GetAllUsersRepositoryResponse.UserReponse(
                    u.Id
                    , u.UserName
                    , u.Email
                    , u.FirstName
                    , u.LastName
                    , u.UserRoles.Select(c => GetAllUsersRepositoryResponse.RoleRepsonse.CastFrom(c.Role)).ToList())
                )
                .ToListAsync();

            return Result.Success(new GetAllUsersRepositoryResponse(users));
        });
    }

    public async Task<Result<GetUserByIdRepositoryResponse>> GetUserByIdAsync(GetUserByIdRepositoryRequest request)
    {
        return await ExecuteAsync<GetUserByIdRepositoryResponse>(async () =>
        {
            var user = await userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user == null)
            {
                return Result.Failure<GetUserByIdRepositoryResponse>(Errors.User.UserNotExist);
            }

            return Result.Success(GetUserByIdRepositoryResponse.CastFrom(user));
        });
    }

    public async Task<Result<Guid>> CreateUserAsync(CreateUserRepositoryRequest request)
    {
        return await ExecuteAsync<Guid>(async () =>
        {
            var userResult = User.Create(request.ToObject());
            if (userResult.IsFailure)
            {
                return Result.Failure<Guid>(userResult.Errors);
            }

            var user = userResult.Value;

            await userStore.SetUserNameAsync(user, request.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, request.Email, CancellationToken.None);

            var identityResult = await userManager.CreateAsync(user, request.Password);
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

    public async Task<Result> UpdateUserAsync(UpdateUserRepositoryRequest request)
    {
        return await ExecuteAsync(async () =>
        {
            var user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return Result.Failure(Errors.User.UserNotExist);
            }

            user.Update(request.ToObject());

            IdentityResult result = await userManager.UpdateAsync(user);

            return Result.Success();
        });
    }

    public async Task<Result<UpdatePasswordRepositoryResponse>> UpdatePasswordAsync(UpdatePasswordRepositoryRequest request)
    {
        return await ExecuteAsync<UpdatePasswordRepositoryResponse>(async () =>
        {
            var user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return Result.Failure<UpdatePasswordRepositoryResponse>(Errors.User.UserNotExist);

            var updatePasswordResult = user.Update(request.ToObject(), passwordHasher);
            if (updatePasswordResult.IsFailure) return Result.Failure<UpdatePasswordRepositoryResponse>(updatePasswordResult.Error);

            IdentityResult result = await userManager.UpdateAsync(user);
            return Result.Success<UpdatePasswordRepositoryResponse>(UpdatePasswordRepositoryResponse.CastFrom(user));
        });
    }
}