using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Identity.Domain;
using Onefocus.Identity.Domain.Entities;
using System.Linq;
using Entity = Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.Databases.Repositories;

public interface IUserRepository
{
    Task<Result<CheckPasswordRepositoryResponse>> CheckPasswordAsync(CheckPasswordRepositoryRequest request);
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

        return Result.Success<CheckPasswordRepositoryResponse>(new (request.Email, roles.ToList()));
    }
}