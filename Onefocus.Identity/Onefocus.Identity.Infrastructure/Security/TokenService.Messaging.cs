using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Identity.Domain.Entities;
using Onefocus.Identity.Infrastructure.Databases.Repositories;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Entity = Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.Security;

public sealed record GenerateTokenServiceRequest(string Email, List<string> Roles)
{
    public static GenerateTokenServiceRequest CastFrom(CheckPasswordRepositoryResponse source) => new(source.User.Email ?? string.Empty, source.Roles);
    public static GenerateTokenServiceRequest CastFrom(GetUserByIdRepositoryResponse source) => new(source.User.Email ?? string.Empty, source.Roles);
}

public sealed record GenerateTokenServiceResponse(string AccessToken);