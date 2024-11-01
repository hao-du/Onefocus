using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;
using Onefocus.Wallet.Infrastructure.Repositories.Read;
using Onefocus.Wallet.Infrastructure.Repositories.Write;
using Onefocus.Wallet.Infrastructure.ServiceBus;

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

        services.AddMassTransit(busConfigure =>
        {
            busConfigure.AddConsumer<UserSyncedConsumer>().Endpoint(configure =>
            {
                configure.InstanceId = Guid.NewGuid().ToString();
            });

            busConfigure.UsingRabbitMq((context, configure) =>
            {
                configure.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });

                configure.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();

        return services;
    }
}
