using Onefocus.Common.Abstractions.Messages;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Domain.Messages.Read;

public sealed record GetUserByIdResponseDto(Guid Id, string Email, string FirstName, string LastName, bool ActiveFlag)
{
    public static GetUserByIdResponseDto? CastFrom(User? source)
    {
        if (source == null) return null;
        return new(source.Id, source.Email, source.FirstName, source.LastName, source.ActiveFlag);
    }
}
