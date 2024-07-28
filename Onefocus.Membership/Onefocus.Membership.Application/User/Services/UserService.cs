using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain;
using Onefocus.Membership.Infrastructure.Databases.Repositories.User;
using System.ComponentModel.DataAnnotations;
using Entity = Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Application.User.Services;

internal interface IUserService
{
    Task<Result<GetAllUsersServiceResponse>> GetAllUsersAsync();
    Task<Result> CreateUserAsync(CreateUserServiceRequest request);
    Task<Result> UpdateUserAsync(UpdateUserServiceRequest request);
    Task<Result> UpdatePasswordAsync(UpdatePasswordServiceRequest request);
}

internal sealed class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<Entity.User> _userManager;
    private readonly IUserStore<Entity.User> _userStore;
    private readonly IUserEmailStore<Entity.User> _emailStore;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository
        , UserManager<Entity.User> userManager
        , IUserStore<Entity.User> userStore
        , ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = (IUserEmailStore<Entity.User>)userStore;
        _logger = logger;
    }

    public async Task<Result<GetAllUsersServiceResponse>> GetAllUsersAsync()
    {
        var userResult = await _userRepository.GetAllUsersAsync();
        if (userResult.IsFailure)
        {
            return Result.Failure<GetAllUsersServiceResponse>(userResult.Error);
        }

        return Result.Success<GetAllUsersServiceResponse>(GetAllUsersServiceResponse.Create(userResult.Value));
    }

    public async Task<Result> CreateUserAsync(CreateUserServiceRequest request)
    {
        var validationResult = ValidateCommonProperties(request);
        if (validationResult.IsFailure) return Result.Failure(validationResult.Error);

        return await _userRepository.CreateUserAsync(request.ToRequestObject());
    }

    public async Task<Result> UpdateUserAsync(UpdateUserServiceRequest request)
    {
        if (request.Id == Guid.Empty) return Result.Failure(Errors.User.IdRequired);

        var validationResult = ValidateCommonProperties(request);
        if (ValidateCommonProperties(request).IsFailure) return Result.Failure(validationResult.Error);

        return await _userRepository.UpdateUserAsync(request.ToRequestObject());
    }

    public async Task<Result> UpdatePasswordAsync(UpdatePasswordServiceRequest request)
    {
        if (request.Id == Guid.Empty) return Result.Failure(Errors.User.IdRequired);
        if (string.IsNullOrEmpty(request.Password)) return Result.Failure(Errors.User.PasswordRequired);
        if (string.IsNullOrEmpty(request.ConfirmPassword)) return Result.Failure(Errors.User.ConfirmPasswordRequired);
        if (request.Password.Equals(request.ConfirmPassword)) return Result.Failure(Errors.User.PasswordNotMatchConfirmPassword);

        return await _userRepository.UpdatePasswordAsync(request.ToRequestObject());
    }

    private Result ValidateCommonProperties(UserServiceRequest request)
    {
        if (string.IsNullOrEmpty(request.FirstName))
        {
            return Result.Failure(Errors.User.FirstNameRequired);
        }

        if (string.IsNullOrEmpty(request.LastName))
        {
            return Result.Failure(Errors.User.LastNameRequired);
        }

        if (string.IsNullOrEmpty(request.Email))
        {
            return Result.Failure(Errors.User.EmailRequired);
        }

        var emailAddressAttribute = new EmailAddressAttribute();
        if (!emailAddressAttribute.IsValid(request.Email))
        {
            return Result.Failure(Errors.User.InvalidEmail);
        }

        return Result.Success();
    }
}

