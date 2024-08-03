using Onefocus.Common.Abstractions.Messaging;
using Onefocus.Identity.Infrastructure.Databases.Repositories;
using Entity = Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.Security;

public sealed record GenerateTokenServiceRequest(string Email, List<string> Roles) : IResponseObject<GenerateTokenServiceRequest, CheckPasswordRepositoryResponse>
{
    public static GenerateTokenServiceRequest Create(CheckPasswordRepositoryResponse source) => new(source.Email, source.Roles);
}

public sealed record GenerateTokenServiceResponse(string AccessToken);