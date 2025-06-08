using Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Domain.Messages;

public sealed record GetAllUsersResponseDto(IReadOnlyList<User> Users);

