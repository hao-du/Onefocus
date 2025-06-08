using Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Application.Contracts;

public sealed record GetAllUsersResponseDto(IReadOnlyList<User> Users);

