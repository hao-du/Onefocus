﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onefocus.Common.Configurations;
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
        var readDatabaseConnectionString = configuration.GetConnectionString("WalletDatabase");
        var writeDatabaseConnectionString = configuration.GetConnectionString("WalletDatabase");

        services.AddDbContext<WalletReadDbContext>(option =>
        {
            option.UseNpgsql(readDatabaseConnectionString);
            option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        services.AddDbContext<WalletWriteDbContext>(option =>
            option.UseNpgsql(writeDatabaseConnectionString)
        );

        IMessageBrokerSettings messageBrokerSettings = configuration.GetSection(IMessageBrokerSettings.SettingName).Get<MessageBrokerSettings>()!;
        services.AddMassTransit(busConfigure =>
        {
            busConfigure.AddConsumer<UserSyncedConsumer>().Endpoint(configure =>
            {
                configure.InstanceId = messageBrokerSettings.InstanceId;
            });

            busConfigure.UsingRabbitMq((context, configure) =>
            {
                configure.Host(new Uri(messageBrokerSettings.Host), host =>
                {
                    host.Username(messageBrokerSettings.UserName);
                    host.Password(messageBrokerSettings.Password);
                });

                configure.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();

        return services;
    }
}
