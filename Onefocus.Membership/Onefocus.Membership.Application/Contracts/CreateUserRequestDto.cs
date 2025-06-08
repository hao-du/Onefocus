using Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Application.Contracts;

public sealed record CreateUserRequestDto(User User, string Password);

