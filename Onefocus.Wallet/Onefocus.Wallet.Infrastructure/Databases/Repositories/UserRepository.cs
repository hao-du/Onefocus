using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Membership.Domain.Entities;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts;
using System.Linq;
using Entity = Onefocus.Membership.Domain.Entities;

namespace Onefocus.Wallet.Infrastructure.Databases.Repositories;

public interface IUserRepository: IRepository<User>
{
    Task<Result<GetAllUsersRepositoryResponse>> GetAllUsersAsync();
    Task<Result<GetUserByIdRepositoryResponse?>> GetUserByIdAsync(GetUserByIdRepositoryRequest request);
    Task<Result<UpsertUserRepositoryResponse>> UpsertUserAsync(UpsertUserRepositoryRequest request);
}

public sealed class UserRepository : IUserRepository
{
    private readonly WalletDbContext _context;

    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        ILogger<UserRepository> logger
        , WalletDbContext context
    )
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<GetAllUsersRepositoryResponse>> GetAllUsersAsync()
    {
        try
        {
            var users = await _context.User.Where(u => u.ActiveFlag).ToListAsync();

            return Result.Success<GetAllUsersRepositoryResponse>(GetAllUsersRepositoryResponse.Create(users));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<GetAllUsersRepositoryResponse>(CommonErrors.InternalServer);
        }
    }

    public async Task<Result<GetUserByIdRepositoryResponse?>> GetUserByIdAsync(GetUserByIdRepositoryRequest request)
    {
        try
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == request.Id);

            return Result.Success<GetUserByIdRepositoryResponse?>(GetUserByIdRepositoryResponse.Create(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<GetUserByIdRepositoryResponse?>(CommonErrors.InternalServer);
        }
    }

    public async Task<Result<UpsertUserRepositoryResponse>> UpsertUserAsync(UpsertUserRepositoryRequest request)
    {
        try
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == request.Id);
            if (user == null)
            {
                var userResult = request.ToRequestObject();
                if (userResult.IsFailure)
                {
                    return Result.Failure<UpsertUserRepositoryResponse>(userResult.Error);
                }
                user = userResult.Value;

                _context.Add(user);
            }
            else
            {
                user.Update(request.Email, request.FirstName, request.LastName, request.ActionFlag, request.ActionBy);
            }

            _context.SaveChanges();

            return Result.Success<UpsertUserRepositoryResponse>(new(user.Id));
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<UpsertUserRepositoryResponse>(CommonErrors.InternalServer);
        }
    }
}