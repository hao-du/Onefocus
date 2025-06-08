using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.Contracts;
using Onefocus.Membership.Application.Interfaces.Repositories;
using Onefocus.Membership.Domain;
using Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Infrastructure.Databases.Repositories;

public sealed class UserRepository(UserManager<User> userManager
        , IUserStore<User> userStore
        , ILogger<UserRepository> logger) : BaseRepository<UserRepository>(logger), IUserRepository
{
    private readonly IUserEmailStore<User> _emailStore = (IUserEmailStore<User>)userStore;

    public async Task<Result<GetAllUsersResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync<GetAllUsersResponseDto>(async () =>
        {
            var users = await userManager.Users.AsNoTracking().ToListAsync(cancellationToken);

            return Result.Success<GetAllUsersResponseDto>(new(users));
        });
    }

    public async Task<Result<GetUserByIdResponseDto>> GetUserByIdAsync(GetUserByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync<GetUserByIdResponseDto>(async () =>
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (user == null)
            {
                return Result.Failure<GetUserByIdResponseDto>(Errors.User.UserNotExist);
            }

            return Result.Success<GetUserByIdResponseDto>(new(user));
        });
    }

    public async Task<Result> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var user = request.User;

            await userStore.SetUserNameAsync(user, request.User.Email, cancellationToken);
            await _emailStore.SetEmailAsync(user, request.User.Email, cancellationToken);

            var identityResult = await userManager.CreateAsync(user, request.Password);
            if (!identityResult.Succeeded) return GetIdentityErrorResult(identityResult);

            return Result.Success();
        });
    }

    public async Task<Result> UpdateUserAsync(UpdateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var identityResult = await userManager.UpdateAsync(request.User);
            if (!identityResult.Succeeded) return GetIdentityErrorResult(identityResult);

            return Result.Success();
        });
    }

    private static Result GetIdentityErrorResult(IdentityResult identityResult)
    {
        var identityErrors = identityResult.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
        if (identityErrors.Count > 0)
        {
            return Result.Failure(identityErrors);
        }
        return Result.Failure(CommonErrors.Unknown);
    }
}