using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Membership.Domain.Entities;
using Onefocus.Common.Constants;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts;
using Onefocus.Wallet.Infrastructure.Databases.Repositories;

namespace Onefocus.Wallet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<WalletDbContext>(option =>
            option.UseNpgsql(configuration.GetConnectionString("WalletDatabase")));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
