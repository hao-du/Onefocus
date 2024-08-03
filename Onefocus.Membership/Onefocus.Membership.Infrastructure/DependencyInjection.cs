using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Membership.Domain.Entities;
using Onefocus.Membership.Infrastructure.Databases.DbContexts;
using Onefocus.Membership.Infrastructure.Databases.Repositories;
using Onefocus.Membership.Infrastructure.Databases.Repositories.User;
namespace Onefocus.Membership.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MembershipDbContext>(option =>
            option.UseNpgsql(configuration.GetConnectionString("MembershipDatabase")));

        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<MembershipDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
