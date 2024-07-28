using Onefocus.Common.Abstractions.Messaging;
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

public sealed record GetAllUsersItemServiceResponse(Guid Id, string? UserName, string? Email, string FirstName, string LastName) 
    : IResponseObject<GetAllUsersItemServiceResponse, UserRepo.GetAllUsersUserItemRepositoryResponse>
{
    public static GetAllUsersItemServiceResponse Create(UserRepo.GetAllUsersUserItemRepositoryResponse source) 
        => new (source.Id, source.UserName, source.Email, source.FirstName, source.LastName);
}
public sealed record GetAllUsersServiceResponse(List<GetAllUsersItemServiceResponse> Users) 
    : IResponseObject<GetAllUsersServiceResponse, UserRepo.GetAllUsersRepositoryResponse>
{
    public static GetAllUsersServiceResponse Create(UserRepo.GetAllUsersRepositoryResponse source) 
        => new (source.Users.Select(user => GetAllUsersItemServiceResponse.Create(user)).ToList());
}

