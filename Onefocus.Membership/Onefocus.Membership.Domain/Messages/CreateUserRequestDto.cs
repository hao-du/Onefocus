using Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Domain.Messages;

public sealed record CreateUserRequestDto(User User, string Password);

