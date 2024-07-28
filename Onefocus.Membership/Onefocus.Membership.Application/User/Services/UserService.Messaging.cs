using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Membership.Domain.Entities;
using Onefocus.Membership.Infrastructure.Databases.Repositories.User;
using static Onefocus.Membership.Application.User.Services.GetAllUsersServiceResponse;
using UserRepo = Onefocus.Membership.Infrastructure.Databases.Repositories.User;

namespace Onefocus.Membership.Application.User.Services;

public abstract record UserServiceRequest(string Email, string FirstName, string LastName);

public sealed record CreateUserServiceRequest(string Email, string FirstName, string LastName, string Password) 
    : UserServiceRequest(Email, FirstName, LastName), IRequestObject<UserRepo.CreateUserRepositoryRequest>
{
    public UserRepo.CreateUserRepositoryRequest ToRequestObject() => new (Email, FirstName, LastName, Password);
}

public sealed record UpdateUserServiceRequest(Guid Id, string Email, string FirstName, string LastName) 
    : UserServiceRequest(Email, FirstName, LastName), IRequestObject<UserRepo.UpdateUserRepositoryRequest>
{
    public UserRepo.UpdateUserRepositoryRequest ToRequestObject() => new (Id, Email, FirstName, LastName);
}

public sealed record UpdatePasswordServiceRequest(Guid Id, string Password, string ConfirmPassword) 
    : IRequestObject<UserRepo.UpdatePasswordRepositoryRequest>
{
    public UserRepo.UpdatePasswordRepositoryRequest ToRequestObject() => new (Id, Password);
}

public sealed record GetAllUsersServiceResponse(List<UserResponse> Users) 
    : IResponseObject<GetAllUsersServiceResponse, UserRepo.GetAllUsersRepositoryResponse>
{
    public static GetAllUsersServiceResponse Create(UserRepo.GetAllUsersRepositoryResponse source) 
        => new (source.Users.Select(user => UserResponse.Create(user)).ToList());

    public sealed record UserResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName, IReadOnlyList<RoleRepsonse> Roles)
    : IResponseObject<UserResponse, UserRepo.GetAllUsersRepositoryResponse.UserReponse>
    {
        public static UserResponse Create(GetAllUsersRepositoryResponse.UserReponse source)
        {
            List<RoleRepsonse> roles = source.Roles.Select(r => RoleRepsonse.Create(r)).ToList();

            return new(source.Id, source.UserName, source.Email, source.FirstName, source.LastName, roles);
        }
    }

    public sealed record RoleRepsonse(Guid Id, string? RoleName) : IResponseObject<RoleRepsonse, GetAllUsersRepositoryResponse.RoleRepsonse>
    {
        public static RoleRepsonse Create(GetAllUsersRepositoryResponse.RoleRepsonse source) => new(source.Id, source.RoleName);
    }
}

