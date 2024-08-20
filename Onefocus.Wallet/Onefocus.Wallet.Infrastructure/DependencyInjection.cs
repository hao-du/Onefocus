using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts;
using Onefocus.Wallet.Infrastructure.Repositories.Read;
using Onefocus.Wallet.Infrastructure.Repositories.Write;

namespace Onefocus.Wallet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<WalletReadDbContext>(option =>
        {
            option.UseNpgsql(configuration.GetConnectionString("WalletDatabase"));
            option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddDbContext<WalletWriteDbContext>(option =>
            option.UseNpgsql(configuration.GetConnectionString("WalletDatabase")));

        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();

        return services;
    }
}
