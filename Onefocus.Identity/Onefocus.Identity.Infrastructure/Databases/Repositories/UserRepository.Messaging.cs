using Onefocus.Common.Abstractions.Messaging;
using Entity = Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.Databases.Repositories;

public sealed record CheckPasswordRepositoryRequest(string Email, string Password);
public sealed record CheckPasswordRepositoryResponse(string Email, List<string> Roles);