using Onefocus.Common.Abstractions.Messages;
using Onefocus.Wallet.Domain.Entities.Read;
using static Onefocus.Wallet.Domain.Messages.Read.GetAllUsersResponseDto;

namespace Onefocus.Wallet.Domain.Messages.Read;

public sealed record GetAllUsersResponseDto(List<UserResponse> Users)
{
    public static GetAllUsersResponseDto CastFrom(List<User> source)
    {
        var users = new List<UserResponse>();
        if (source != null && source.Any())
        {
            users = source.Select(s => new UserResponse(s.Id, s.Email, s.FirstName, s.LastName, s.ActiveFlag)).ToList();
        }

        return new(users);
    }

    public sealed record UserResponse(Guid Id, string Email, string FirstName, string LastName, bool ActiveFlag);
}
