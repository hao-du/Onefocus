using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Identity.Infrastructure.Databases.DbContexts;
using Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(option =>
            option.UseNpgsql(configuration.GetConnectionString("IdentityDatabase")));

        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
